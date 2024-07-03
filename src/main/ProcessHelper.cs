using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal static class ProcessHelper
    {
        public static async Task<IGranny> ObtainWithAggregateParamsAsync<T, TIGranny, TParameterSet, TResult>(
            T granny,
            Ensemble ensemble,
            IPrimitiveSet primitives,
            TParameterSet parameters,
            Action<TIGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where T : TIGranny, new()
            where TIGranny : IGranny<TIGranny, TParameterSet>
            where TParameterSet : IAggregateParameterSet
        {
            TIGranny result = await granny.ObtainAsync(
                ensemble,
                primitives,
                parameters
                );

            resultUpdater(result, tempResult);

            return result;
        }

        public static IGranny TryParse<T, TIGranny, TParameterSet, TResult>(
            T granny,
            Ensemble ensemble,
            IPrimitiveSet primitives,
            TParameterSet parameters,
            Action<TIGranny, TResult> resultUpdater,
            TResult tempResult
        )
            where T : TIGranny, new()
            where TIGranny : IGranny<TIGranny, TParameterSet>
            where TParameterSet : IAggregateParameterSet
        {
            IGranny result = null;

            if (granny.TryParse(
                ensemble,
                primitives,
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
