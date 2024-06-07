using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    public static class Extensions
    {
        #region IGranny
        /// <summary>
        /// Retrieves granny from ensemble if present; Otherwise, retrieves it from persistence or builds it, and adds it to the ensemble.
        /// </summary>
        /// <typeparam name="TGranny"></typeparam>
        /// <typeparam name="TParameterSet"></typeparam>
        /// <param name="granny"></param>
        /// <param name="ensemble"></param>
        /// <param name="parameterSet"></param>
        /// <param name="neuronRepository"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async static Task<Neuron> ObtainAsync<TGranny, TParameterSet>(
                this IGranny<TGranny, TParameterSet> granny, 
                Ensemble ensemble,
                ICoreSet coreSet,
                TParameterSet parameterSet,
                IEnsembleRepository neuronRepository,
                string userId
            ) 
            where TGranny : IGranny<TGranny, TParameterSet>
            where TParameterSet : IParameterSet
        {
            AssertionConcern.AssertArgumentNotNull(ensemble, nameof(ensemble));
            AssertionConcern.AssertArgumentNotNull(parameterSet, nameof(parameterSet));

            Neuron result = null;
            // if target is not in specified ensemble
            if (!granny.TryParse(ensemble, coreSet, parameterSet, out Neuron ensembleParseResult))
            {
                // retrieve target from DB
                var queries = granny.GetQueries(coreSet, parameterSet);
                ensemble.AddReplaceItems(await neuronRepository.GetByQueriesAsync(userId, queries.ToArray()));
                // if target is in DB
                if (granny.TryParse(ensemble, coreSet, parameterSet, out Neuron dbParseResult))
                {
                    result = dbParseResult;
                }
                // else if target is not in DB 
                else
                {
                    // build in ensemble
                    result = await granny.BuildAsync(ensemble, coreSet, parameterSet);
                }
            }
            // if target was found in ensemble
            else if (ensembleParseResult != null)
                result = ensembleParseResult;

            return result;
        }
        #endregion
        
        public static bool HasSameElementsAs<T>(
                this IEnumerable<T> first,
                IEnumerable<T> second
            )
        {
            var firstMap = first
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
            var secondMap = second
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
            return
                firstMap.Keys.All(x =>
                    secondMap.Keys.Contains(x) && firstMap[x] == secondMap[x]
                ) &&
                secondMap.Keys.All(x =>
                    firstMap.Keys.Contains(x) && secondMap[x] == firstMap[x]
                );
        }
    }
}
