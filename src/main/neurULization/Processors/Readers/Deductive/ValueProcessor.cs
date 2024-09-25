using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ValueProcessor : IValueProcessor
    {
        private readonly IInstantiatesClassProcessor instantiatesClassProcessor;

        public ValueProcessor(IInstantiatesClassProcessor instantiatesClassProcessor)
        {
            this.instantiatesClassProcessor = instantiatesClassProcessor;
        }

        private static IEnumerable<IGreatGrannyInfo<IValue>> CreateGreatGrannies(
            IInstantiatesClassProcessor instantiatesClassProcessor,
            IValueParameterSet parameters
        ) =>
            new IGreatGrannyInfo<IValue>[]
            {
                new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IValue>(
                    instantiatesClassProcessor,
                    () => CreateInstantiatesClassParameterSet(parameters),
                    (g, r) => r.InstantiatesClass = g
                )
            };

        public IEnumerable<IGrannyQuery> GetQueries(IValueParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet>(
                    instantiatesClassProcessor,
                    (n) => CreateInstantiatesClassParameterSet(parameters)
                ),
                new GrannyQueryBuilder(
                    (n) => new NeuronQuery()
                    {
                        Id = parameters.ValueMatchBy == ValueMatchBy.Id ?
                            new[] { parameters.Value.Id.ToString() } :
                            null,
                        Tag = parameters.ValueMatchBy == ValueMatchBy.Tag ?
                            new[] { parameters.Value.Tag } :
                            null,
                        DirectionValues = DirectionValues.Outbound,
                        Depth = n.Last() != null ? 1 : (int?) null,
                        TraversalDepthPostsynaptic = n.Last() != null ?
                            new[] {
                                // 1 edge away and should have postsynaptic of previous granny
                                new DepthIdsPair {
                                    Depth = 1,
                                    Ids = new[] { n.Last().Neuron.Id }
                                },
                            } :
                            null
                    },
                    false
                )
            };

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, IValueParameterSet parameters, out IValue result)
        {
            result = null;

            new Value().AggregateTryParse(
                ValueProcessor.CreateGreatGrannies(
                    this.instantiatesClassProcessor,
                    parameters
                ),
                new IGreatGrannyProcess<IValue>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IValue>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                out IValue tempResult,
                false
            );

            if (tempResult != null)
                tempResult.TryParseCore(
                    ensemble,
                    // start from InstantiatesClass neuron
                    new[] { tempResult.InstantiatesClass.Neuron.Id },
                    new[] { new LevelParser(
                            new PresynapticBy(n =>
                                parameters.ValueMatchBy == ValueMatchBy.Id ?
                                n.Id == parameters.Value.Id :
                                n.Tag == parameters.Value.Tag
                            )
                        )
                    },
                    (n) => tempResult.Neuron = n,
                    ref result
                );

            return result != null;
        }
    }
}
