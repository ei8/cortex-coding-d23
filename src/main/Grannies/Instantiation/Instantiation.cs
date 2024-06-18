using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Instantiation : IInstantiation
    {
        public async Task<IInstantiation> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IInstantiationParameterSet parameters)
        {
            var result = new Instantiation();
            result.Neuron = ensemble.Obtain(parameters.Value);
            result.InstantiatesClass = await new InstantiatesClass().ObtainAsync(
                ensemble,
                primitives,
                new InstantiatesClassParameterSet(
                    parameters.Class,
                    parameters.EnsembleRepository,
                    parameters.UserId
                ),
                parameters.EnsembleRepository,
                parameters.UserId
                );
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.InstantiatesClass.Neuron.Id));
            return result;
        }

        public IEnumerable<GrannyQuery> GetQueries(IPrimitiveSet primitives, IInstantiationParameterSet parameters) =>
            new[] {
                new GrannyQuery(
                    (ps) => new InstantiatesClass().GetQueries(
                            primitives,
                            (IInstantiatesClassParameterSet) ps
                        ).Single().GetQuery(),
                    Instantiation.CreateInstantiatesParameterSet(parameters),
                    (Ensemble e, IPrimitiveSet prs, IParameterSet ps, out IGranny r) => 
                        ((IInstantiatesClass) new InstantiatesClass()).TryParseGranny(
                            e, 
                            prs, 
                            (IInstantiatesClassParameterSet) ps, 
                            out r
                            )
                ),
                new GrannyQuery(
                    (n) => new NeuronQuery()
                    {
                        Id = parameters.MatchingNeuronProperty == InstantiationMatchingNeuronProperty.Id ? 
                            new[] { parameters.Value.Id.ToString() } : 
                            Array.Empty<string>(),
                        TagContains = parameters.MatchingNeuronProperty == InstantiationMatchingNeuronProperty.Tag ?
                            new[] { parameters.Value.Tag } : 
                            Array.Empty<string>(),
                        DirectionValues = DirectionValues.Outbound,
                        Depth = 1,
                        TraversalDepthPostsynaptic = new[] {
                            // 1 edge away and should have postsynaptic of previous granny
                            new DepthIdsPair {
                                Depth = 1,
                                Ids = new[] { n.Id }
                            },
                        }
                    }
                )
            };

        private static InstantiatesClassParameterSet CreateInstantiatesParameterSet(IInstantiationParameterSet parameters)
        {
            return new InstantiatesClassParameterSet(
                parameters.Class,
                parameters.EnsembleRepository,
                parameters.UserId
            );
        }

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IInstantiationParameterSet parameters, out IInstantiation result)
        {
            result = null;

            var tempResult = new Instantiation();

            if (new InstantiatesClass().TryParse(
                ensemble, 
                primitives, 
                Instantiation.CreateInstantiatesParameterSet(parameters), 
                out IInstantiatesClass ic
                ))
            {
                tempResult.InstantiatesClass = ic;

                this.TryParseCore(
                    parameters,
                    ensemble,
                    tempResult,
                    // start from InstantiatesClass neuron
                    new[] { tempResult.InstantiatesClass.Neuron.Id },
                    new[] { new LevelParser(
                            new PresynapticBy(n => 
                                parameters.MatchingNeuronProperty == InstantiationMatchingNeuronProperty.Id ? 
                                n.Id == parameters.Value.Id :
                                n.Tag == parameters.Value.Tag
                            )
                        )
                    },
                    (n) => tempResult.Neuron = n,
                    ref result
                );
            }

            return result != null;
        }

        public IInstantiatesClass InstantiatesClass { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
