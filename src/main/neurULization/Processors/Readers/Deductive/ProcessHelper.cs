using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    internal static class ProcessHelper
    {
        public static IGranny TryParse<TGranny, TGrannyProcessor, TParameterSet, TAggregate>(
            TGrannyProcessor grannyProcessor,
            Ensemble ensemble,
            TParameterSet parameters,
            Action<TGranny, TAggregate> aggregateUpdater,
            TAggregate aggregate
        )
            where TGranny : IGranny
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>, IGrannyReadProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IDeductiveParameterSet
        {
            IGranny result = null;

            if (grannyProcessor.TryParse(
                ensemble,
                parameters,
                out TGranny gr)
                )
            {
                aggregateUpdater(gr, aggregate);
                result = gr;
            }

            return result;
        }
    }
}
