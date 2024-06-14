using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Subordination : ISubordination
    {
        public async Task<ISubordination> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, ISubordinationParameterSet parameters)
        {
            var result = new Subordination();
            var subordination = ensemble.Obtain(primitives.Subordination);
            result.Head = await new Head().ObtainAsync(
                ensemble,
                primitives,
                new HeadParameterSet(
                    parameters.HeadParameters.Value
                ),
                parameters.EnsembleRepository,
                parameters.UserId
                );

            var ides = new List<IDependent>();
            foreach (var dp in parameters.DependentsParameters)
            {
                var ide = await new Dependent().ObtainAsync(
                    ensemble,
                    primitives,
                    new DependentParameterSet(
                        dp.Value,
                        dp.Type
                        ),
                    parameters.EnsembleRepository,
                    parameters.UserId
                    );
                ides.Add(ide);
            }
            result.Dependents = ides;
            result.Neuron = ensemble.Obtain(Neuron.CreateTransient(null, null, null));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, subordination.Id));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Head.Neuron.Id));
            ides.ForEach(ide => ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, ide.Neuron.Id)));

            return result;
        }

        public IEnumerable<GrannyQuery> GetQueries(IPrimitiveSet primitives, ISubordinationParameterSet parameters) =>
            new[] {
                new GrannyQuery(
                    new NeuronQuery()
                    {
                        Id = parameters.DependentsParameters.Select(dp => dp.Value.Id.ToString()),
                        DirectionValues = DirectionValues.Any,
                        Depth = 4,
                        TraversalDepthPostsynaptic = new[] {
                            // 4 edges away and should have postsynaptic of unit or instantiates
                            new DepthIdsPair {
                                Depth = 4,
                                Ids = new[] {
                                    parameters.HeadParameters.Value.Id,
                                    primitives.Unit.Id
                                }
                            },
                            // 3 edges away and should have postsynaptic of subordination
                            new DepthIdsPair {
                                Depth = 3,
                                Ids = new[] {
                                    primitives.Subordination.Id
                                }
                            },
                            // 2 edges away and should have postsynaptic of direct object
                            new DepthIdsPair {
                                Depth = 2,
                                Ids = new[] {
                                    primitives.DirectObject.Id
                                }
                            }
                        }
                    }
                )
            };

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, ISubordinationParameterSet parameters, out ISubordination result)
        {
            result = null;

            var tempResult = new Subordination();

            var ides = new List<IDependent>();
            foreach (var dp in parameters.DependentsParameters)
            {
                if (new Dependent().TryParse(ensemble, primitives, dp, out IDependent ide))
                    ides.Add(ide);
            }

            if (ides.Count == parameters.DependentsParameters.Count())
            {
                tempResult.Dependents = ides;
                if (new Head().TryParse(ensemble, primitives, parameters.HeadParameters, out IHead head))
                {
                    tempResult.Head = head;

                    this.TryParseCore(
                        parameters,
                        ensemble,
                        tempResult,
                        // start from the head neuron
                        new[] { tempResult.Head.Neuron },
                        new[]
                        {
                            // get the presynaptic via the siblings of the head and subordination
                            new LevelParser(new PresynapticBySibling(
                                ides.Select(i => i.Neuron).Concat(new[] { primitives.Subordination }).ToArray()
                                ))
                        },
                        (n) => tempResult.Neuron = n,
                        ref result
                        );
                }
            }

            return result != null;
        }

        public IHead Head { get; private set; }

        public IEnumerable<IDependent> Dependents { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
