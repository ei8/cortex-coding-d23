using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    internal static class ProcessHelper
    {
        public static IGranny TryParse<TGranny, TGrannyReadProcessor, TReadParameterSet, TResult>(
            TGrannyReadProcessor grannyReadProcessor,
            Ensemble ensemble,
            Id23neurULizerReadOptions options,
            TReadParameterSet readParameters,
            Action<TGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where TGranny : IGranny
            where TGrannyReadProcessor : IGrannyReadProcessor<TGranny, TReadParameterSet>
            where TReadParameterSet : IReadParameterSet
        {
            IGranny result = null;

            if (grannyReadProcessor.TryParse(
                ensemble,
                options,
                readParameters,
                out TGranny gr)
                )
            {
                resultUpdater(gr, tempResult);
                result = gr;
            }

            return result;
        }
    }
}
