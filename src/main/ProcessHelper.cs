using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal static class ProcessHelper
    {
        public static async Task<IGranny> ObtainWithAggregateParamsAsync<T, TIGranny, TParameterSet, TResult>(
            T granny,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TParameterSet parameters,
            Action<TIGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where T : TIGranny, new()
            where TIGranny : IGranny<TIGranny, TParameterSet>
            where TParameterSet : IParameterSet
        {
            TIGranny result = await granny.ObtainAsync(
                ensemble,
                options,
                parameters
                );

            resultUpdater(result, tempResult);

            return result;
        }

        public static async Task<IGranny> ObtainAsync<T, TIGranny, TParameterSet, TResult>(
            T granny,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TParameterSet parameters,
            Action<TIGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where T : TIGranny, new()
            where TIGranny : IGranny<TIGranny, TParameterSet>
            where TParameterSet : IParameterSet
        {
            TIGranny result = await granny.ObtainAsync(
                new ObtainParameters(
                    ensemble,
                    options
                ),
                parameters
                );

            resultUpdater(result, tempResult);

            return result;
        }

        public static IGranny TryParse<T, TIGranny, TParameterSet, TResult>(
            T granny,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            TParameterSet parameters,
            Action<TIGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where T : TIGranny, new()
            where TIGranny : IGranny<TIGranny, TParameterSet>
            where TParameterSet : IParameterSet
        {
            IGranny result = null;

            if (granny.TryParse(
                ensemble,
                options,
                parameters,
                out TIGranny gr)
                )
            {
                resultUpdater(gr, tempResult);
                result = gr;
            }

            return result;
        }
    }
}
