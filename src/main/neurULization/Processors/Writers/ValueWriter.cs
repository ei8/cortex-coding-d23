using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ValueWriter : IValueWriter
    {
        private readonly IInstantiatesClassWriter instantiatesClassWriter;
        private readonly Readers.Deductive.IValueReader reader;

        public ValueWriter(
            IInstantiatesClassWriter instantiatesClassWriter,
            Readers.Deductive.IValueReader reader
        )
        {
            this.instantiatesClassWriter = instantiatesClassWriter;
            this.reader=reader;
        }

        public IValue Build(Ensemble ensemble, IValueParameterSet parameters) =>
            new Value().AggregateBuild(
                ValueWriter.CreateGreatGrannies(this.instantiatesClassWriter, parameters),
                new IGreatGrannyProcess<IValue>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassWriter, IInstantiatesClassParameterSet, IValue>(
                        ProcessHelper.ParseBuild
                    )
                },
                ensemble,
                () => ensemble.AddOrGetIfExists(parameters.Value),
                (g) => new[] { g.InstantiatesClass.Neuron }
            );

        private static IEnumerable<IGreatGrannyInfo<IValue>> CreateGreatGrannies(
            IInstantiatesClassWriter instantiatesClassWriter,
            IValueParameterSet parameters
        ) =>
            new IGreatGrannyInfo<IValue>[]
            {
                new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassWriter, IInstantiatesClassParameterSet, IValue>(
                    instantiatesClassWriter,
                    () => ValueWriter.CreateInstantiatesClassParameterSet(parameters),
                    (g, r) => r.InstantiatesClass = g
                )
            };

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        public IGrannyReader<IValue, IValueParameterSet> Reader => this.reader;
    }
}
