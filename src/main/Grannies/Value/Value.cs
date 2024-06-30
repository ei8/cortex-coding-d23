using ei8.Cortex.Coding.d23.Queries;
using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Value : IValue
    {
        public async Task<IValue> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IValueParameterSet parameters)
        {
            var result = new Value();
            result.Neuron = ensemble.Obtain(parameters.Value);
            result.InstantiatesClass = await new InstantiatesClass().ObtainAsync(
                ensemble,
                primitives,
                Value.CreateInstantiatesClassParameterSet(parameters)
                );
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.InstantiatesClass.Neuron.Id));
            return result;
        }

        public IEnumerable<IGrannyQuery> GetQueries(IPrimitiveSet primitives, IValueParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<InstantiatesClass, IInstantiatesClass, IInstantiatesClassParameterSet>(
                    (n) => Value.CreateInstantiatesClassParameterSet(parameters)
                ),
                new GrannyQueryBuilder(
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

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class,
                parameters.EnsembleRepository,
                parameters.UserId
            );

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IValueParameterSet parameters, out IValue result)
        {
            result = null;

            var tempResult = new Value();

            if (new InstantiatesClass().TryParse(
                ensemble, 
                primitives, 
                Value.CreateInstantiatesClassParameterSet(parameters), 
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
