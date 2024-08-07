using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    internal static class ProcessHelper
    {
        public static async Task<IGranny> ObtainWithAggregateParamsAsync<TGranny, TGrannyWriteProcessor, TWriteParameterSet, TResult>(
            TGrannyWriteProcessor grannyWriteProcessor,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TWriteParameterSet writeParameters,
            Action<TGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where TGranny : IGranny
            where TGrannyWriteProcessor : IGrannyWriteProcessor<TGranny, TWriteParameterSet>
            where TWriteParameterSet : IWriteParameterSet
        {
            TGranny result = await grannyWriteProcessor.ObtainAsync<TGranny, TGrannyWriteProcessor, TWriteParameterSet>(
                ensemble,
                options,
                writeParameters
                );

            resultUpdater(result, tempResult);

            return result;
        }

        public static async Task<IGranny> ObtainAsync<TGranny, TGrannyWriteProcessor, TWriteParameterSet, TResult>(
            TGrannyWriteProcessor grannyWriteProcessor,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TWriteParameterSet writeParameters,
            Action<TGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where TGranny : IGranny
            where TGrannyWriteProcessor : IGrannyWriteProcessor<TGranny, TWriteParameterSet>
            where TWriteParameterSet : IWriteParameterSet
        {
            TGranny result = await grannyWriteProcessor.ObtainAsync<TGranny, TGrannyWriteProcessor, TWriteParameterSet>(
                new ProcessParameters(
                    ensemble,
                    options
                ),
                writeParameters
                );

            resultUpdater(result, tempResult);

            return result;
        }

        public static IGranny TryParse<TGranny, TGrannyWriteProcessor, TWriteParameterSet, TResult>(
            TGrannyWriteProcessor grannyWriteProcessor,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TWriteParameterSet writeParameters,
            Action<TGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where TGranny : IGranny
            where TGrannyWriteProcessor : IGrannyWriteProcessor<TGranny, TWriteParameterSet>
            where TWriteParameterSet : IWriteParameterSet
        {
            IGranny result = null;

            if (grannyWriteProcessor.TryParse(
                ensemble,
                options,
                writeParameters,
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
