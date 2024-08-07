using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class ValueProcessor : IValueProcessor
    {
        private readonly IInstantiatesClassProcessor instantiatesClassProcessor;

        public ValueProcessor(IInstantiatesClassProcessor instantiatesClassProcessor)
        {
            this.instantiatesClassProcessor = instantiatesClassProcessor;
        }

        public async Task<IValue> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IValueParameterSet parameters) =>
            await new Value().AggregateBuildAsync(
                CreateGreatGrannies(options, parameters),
                new IGreatGrannyProcessAsync<IValue>[]
                {
                    new GreatGrannyWriteProcessAsync<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IValue>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                options,
                () => ensemble.Obtain(parameters.Value),
                (g) => new[] { g.InstantiatesClass.Neuron }
            );

        private IEnumerable<IGreatGrannyInfo<IValue>> CreateGreatGrannies(Id23neurULizerOptions options, IValueParameterSet parameters) =>
            new IGreatGrannyInfo<IValue>[]
            {
                new GreatGrannyWriteInfo<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IValue>(
                    instantiatesClassProcessor,
                    (g) => CreateInstantiatesClassParameterSet(parameters),
                    (g, r) => r.InstantiatesClass = g
                )
            };

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerOptions options, IValueParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet>(
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
                        Depth = 1,
                        TraversalDepthPostsynaptic = new[] {
                            // 1 edge away and should have postsynaptic of previous granny
                            new DepthIdsPair {
                                Depth = 1,
                                Ids = new[] { n.Last().Neuron.Id }
                            },
                        }
                    }
                )
            };

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IValueParameterSet parameters, out IValue result)
        {
            result = null;

            new Value().AggregateTryParse(
                CreateGreatGrannies(options, parameters),
                new IGreatGrannyProcess<IValue>[]
                {
                    new GreatGrannyWriteProcess<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IValue>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                options,
                out IValue tempResult,
                false
            );

            if (tempResult != null)
                this.TryParseCore(
                    parameters,
                    ensemble,
                    tempResult,
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
