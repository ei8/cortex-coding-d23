using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
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
            IValueParameterSet parameters,
            Ensemble ensemble
            ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IValue>(
                    instantiatesClassProcessor,
                    () => CreateInstantiatesClassParameterSet(parameters, gc),
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

        public bool TryParse(Ensemble ensemble, Id23neurULizerReadOptions options, IValueParameterSet parameters, out IValue result) =>
            new Value().AggregateTryParse(
                parameters.Granny,
                CreateGreatGrannies(instantiatesClassProcessor, parameters, ensemble),
                new IGreatGrannyProcess<IValue>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IValue>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                options,
                1,
                out result
            );
    }
}
