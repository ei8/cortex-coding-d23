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

        private static IEnumerable<IGreatGrannyInfo<IInstanceValue>> CreateGreatGrannies(
            IInstantiatesClassReader instantiatesClassReader,
            IValueParameterSet parameters
        ) =>
            new IGreatGrannyInfo<IInstanceValue>[]
            {
                new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstanceValue>(
                    instantiatesClassReader,
                    () => ValueReader.CreateInstantiatesClassParameterSet(parameters),
                    (g, r) => r.InstantiatesClass = g
                )
            };

        public IEnumerable<IGrannyQuery> GetQueries(IValueParameterSet parameters) =>
            parameters.Class != null ?
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
                } :
                new[] {
                    new GrannyQuery(
                        new NeuronQuery()
                        {
                            Id = new[] { parameters.Value.Id.ToString () }
                        }
                    )
                };

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, IValueParameterSet parameters, out IValue result)
        {
            result = null;

            if (parameters.Class != null)
            {
                new InstanceValue().AggregateTryParse(
                    ValueReader.CreateGreatGrannies(
                        this.instantiatesClassReader,
                        parameters
                    ),
                    new IGreatGrannyProcess<IInstanceValue>[]
                    {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstanceValue>(
                        ProcessHelper.TryParse
                    )
                    },
                    ensemble,
                    out IInstanceValue tempResult,
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
            }
            else
            {
                result = new Value();
                result.Neuron = parameters.Value;
            }

            return result != null;
        }
    }
}
