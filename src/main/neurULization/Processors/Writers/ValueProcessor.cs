using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ValueProcessor : IValueProcessor
    {
        private readonly IInstantiatesClassProcessor instantiatesClassProcessor;
        private readonly Readers.Deductive.IValueProcessor readerProcessor;

        public ValueProcessor(
            IInstantiatesClassProcessor instantiatesClassProcessor,
            Readers.Deductive.IValueProcessor readerProcessor
        )
        {
            this.instantiatesClassProcessor = instantiatesClassProcessor;
            this.readerProcessor=readerProcessor;
        }

        public async Task<IValue> BuildAsync(Ensemble ensemble, IValueParameterSet parameters) =>
            await new Value().AggregateBuildAsync(
                ValueProcessor.CreateGreatGrannies(this.instantiatesClassProcessor, parameters),
                new IGreatGrannyProcessAsync<IValue>[]
                {
                    new GreatGrannyProcessAsync<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IValue>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                () => ensemble.Obtain(parameters.Value),
                (g) => new[] { g.InstantiatesClass.Neuron }
            );

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

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        public IGrannyReadProcessor<IValue, IValueParameterSet> ReadProcessor => this.readerProcessor;
    }
}
