using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
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
            ensemble.GetPostsynapticNeurons(parameters.Granny.Id).Select(
                gc => new GreatGrannyReadInfo<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IValue>(
                    instantiatesClassProcessor,
                    ValueProcessor.CreateInstantiatesClassParameterSet(parameters, gc),
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
                parameters,
                ValueProcessor.CreateGreatGrannies(this.instantiatesClassProcessor, parameters, ensemble),
                new IGreatGrannyReadProcess<IValue>[]
                {
                    new GreatGrannyReadProcess<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IValue>(
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
