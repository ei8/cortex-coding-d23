using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
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
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet
        {
            TGranny result = await grannyProcessor.ObtainAsync<TGranny, TGrannyProcessor, TParameterSet>(
                ensemble,
                options,
                parameters
                );

            resultUpdater(result, tempResult);

            return result;
        }

        public static async Task<IGranny> ObtainAsync<TGranny, TGrannyProcessor, TParameterSet, TResult>(
            TGrannyProcessor grannyProcessor,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TParameterSet parameters,
            Action<TGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where TGranny : IGranny
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet
        {
            TGranny result = await grannyProcessor.ObtainAsync<TGranny, TGrannyProcessor, TParameterSet>(
                new ObtainParameters(
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
            where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
            where TParameterSet : IParameterSet
        {
            IGranny result = null;

            if (grannyProcessor.TryParse(
                ensemble,
                options,
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
