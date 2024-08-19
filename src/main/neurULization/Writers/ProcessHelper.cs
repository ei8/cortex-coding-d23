using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    internal static class ProcessHelper
    {
        public static async Task<IGranny> ObtainWithAggregateParamsAsync<TGranny, TGrannyProcessor, TParameterSet, TResult>(
            TGrannyProcessor grannyProcessor,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TParameterSet parameters,
            Action<TGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where TGranny : IGranny
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>, IGrannyWriteProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IWriteParameterSet
            where TResult : IGranny
        {
            TGranny result = await grannyProcessor.ObtainAsync<TGranny, TGrannyProcessor, TParameterSet>(
                ensemble,
                (Id23neurULizerWriteOptions) options,
                parameters
                );

            resultUpdater(result, tempResult);

            return result;
        }

        public static async Task<IGranny> ObtainAsync<TGranny, TGrannyProcessor, TParameterSet, TResult>(
            TGrannyProcessor grannyProcessor,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TParameterSet parameters,            Action<TGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where TGranny : IGranny
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>, IGrannyWriteProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IWriteParameterSet
        {
            TGranny result = await grannyProcessor.ObtainAsync<TGranny, TGrannyProcessor, TParameterSet>(
                new ProcessParameters(
                    ensemble,
                    options
                ),
                parameters
                );

            resultUpdater(result, tempResult);

            return result;
        }

        public static IGranny TryParse<TGranny, TGrannyProcessor, TParameterSet, TResult>(
            TGrannyProcessor grannyProcessor,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TParameterSet parameters,
            Action<TGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where TGranny : IGranny
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>, IGrannyWriteProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet, IWriteParameterSet
        {
            IGranny result = null;

            if (grannyProcessor.TryParse(
                ensemble,
                (Id23neurULizerWriteOptions) options,
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
