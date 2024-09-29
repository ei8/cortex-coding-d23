using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

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

        public IValue Build(Ensemble ensemble, IValueParameterSet parameters) =>
            new Value().AggregateBuild(
                ValueProcessor.CreateGreatGrannies(this.instantiatesClassProcessor, parameters),
                new IGreatGrannyProcess<IValue>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IValue>(
                        ProcessHelper.ParseBuild
                    )
                },
                ensemble,
                () => ensemble.AddOrGetIfExists(parameters.Value),
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
                    () => ValueProcessor.CreateInstantiatesClassParameterSet(parameters),
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
