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
        public async Task<ISubordination> BuildAsync(Ensemble ensemble, ICoreSet coreSet, ISubordinationParameterSet parameterSet)
        {
            var result = new Subordination();
            var subordination = ensemble.Obtain(coreSet.Subordination);
            result.Head = await new Head().ObtainAsync(
                ensemble,
                coreSet,
                new HeadParameterSet(
                    parameterSet.HeadParameters.Value
                ),
                parameterSet.NeuronRepository,
                parameterSet.UserId
                );

            var ides = new List<IDependent>();
            foreach (var dp in parameterSet.DependentsParameters)
            {
                var ide = await new Dependent().ObtainAsync(
                    ensemble,
                    coreSet,
                    new DependentParameterSet(
                        dp.Value,
                        dp.Type
                        ),
                    parameterSet.NeuronRepository,
                    parameterSet.UserId
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

        public IEnumerable<NeuronQuery> GetQueries(ICoreSet coreSet, ISubordinationParameterSet parameterSet) =>
            new[] {
                new NeuronQuery()
                {
                    Id = parameterSet.DependentsParameters.Select(dp => dp.Value.Id.ToString()),
                    DirectionValues = DirectionValues.Any,
                    Depth = 3,
                    TraversalMinimumDepthPostsynaptic = new[] {
                        new DepthIdsPair {
                            Depth = 3,
                            Ids = new[] {
                                coreSet.Subordination.Id,
                                parameterSet.HeadParameters.Value.Id
                            }
                        }
                    }
                }
            };

        public bool TryParse(Ensemble ensemble, ICoreSet coreSet, ISubordinationParameterSet parameterSet, out ISubordination result)
        {
            result = null;

            var tempResult = new Subordination();

            var ides = new List<IDependent>();
            foreach (var dp in parameterSet.DependentsParameters)
            {
                if (new Dependent().TryParse(ensemble, coreSet, dp, out IDependent ide))
                    ides.Add(ide);
            }

            if (ides.Count == parameterSet.DependentsParameters.Count())
            {
                tempResult.Dependents = ides;
                if (new Head().TryParse(ensemble, coreSet, parameterSet.HeadParameters, out IHead head))
                {
                    tempResult.Head = head;

                    this.TryParseCore(
                        parameterSet,
                        ensemble,
                        tempResult,
                        tempResult.Dependents.Select(d => d.Neuron),
                        new[]
                        {
                            new LevelParser(new PresynapticBySibling(
                                coreSet.Subordination,
                                parameterSet.HeadParameters.Value
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
