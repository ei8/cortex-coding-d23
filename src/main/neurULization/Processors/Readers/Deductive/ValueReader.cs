using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ValueReader : IValueReader
    {
        private readonly IInstantiatesClassReader instantiatesClassReader;

        public ValueReader(IInstantiatesClassReader instantiatesClassReader)
        {
            this.instantiatesClassReader = instantiatesClassReader;
        }

        private static IEnumerable<IGreatGrannyInfo<IValue>> CreateGreatGrannies(
            IInstantiatesClassReader instantiatesClassReader,
            IValueParameterSet parameters
        ) =>
            new IGreatGrannyInfo<IValue>[]
            {
                new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IValue>(
                    instantiatesClassReader,
                    () => ValueReader.CreateInstantiatesClassParameterSet(parameters),
                    (g, r) => r.InstantiatesClass = g
                )
            };

        public IEnumerable<IGrannyQuery> GetQueries(IValueParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet>(
                    this.instantiatesClassReader,
                    (n) => ValueReader.CreateInstantiatesClassParameterSet(parameters)
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
                ValueReader.CreateGreatGrannies(
                    this.instantiatesClassReader,
                    parameters
                ),
                new IGreatGrannyProcess<IValue>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IValue>(
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
