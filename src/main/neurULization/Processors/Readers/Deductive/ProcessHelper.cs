using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    internal static class ProcessHelper
    {
        public static IGranny TryParse<TGranny, TGrannyProcessor, TParameterSet, TResult>(
            TGrannyProcessor grannyProcessor,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TParameterSet parameters,
            Action<TGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where TGranny : IGranny
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>, IGrannyReadProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IDeductiveParameterSet
        {
            IGranny result = null;

            if (grannyProcessor.TryParse(
                ensemble,
                (Id23neurULizerWriteOptions)options,
                parameters,
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
