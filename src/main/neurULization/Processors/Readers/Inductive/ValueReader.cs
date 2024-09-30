using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
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
            IValueParameterSet parameters,
            Ensemble ensemble
            ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IValue>(
                    instantiatesClassReader,
                    () => ValueReader.CreateInstantiatesClassParameterSet(parameters, gc),
                    (g, r) => r.InstantiatesClass = g
                )
            );

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(
            IValueParameterSet parameters,
            Neuron grannyCandidate
            ) =>
            new InstantiatesClassParameterSet(
                grannyCandidate,
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, IValueParameterSet parameters, out IValue result) =>
            new Value().AggregateTryParse(
                parameters.Granny,
                ValueReader.CreateGreatGrannies(this.instantiatesClassReader, parameters, ensemble),
                new IGreatGrannyProcess<IValue>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IValue>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                1,
                out result
            );
    }
}
