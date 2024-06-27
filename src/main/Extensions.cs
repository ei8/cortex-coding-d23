using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using neurUL.Common;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    public static class Extensions
    {
        #region IGranny
        public async static Task<TGranny> ObtainAsync<TGranny, TParameterSet>(
                this IGranny<TGranny, TParameterSet> granny,
                Ensemble ensemble,
                IPrimitiveSet primitives,
                TParameterSet parameters
            )
            where TGranny : IGranny<TGranny, TParameterSet>
            where TParameterSet : IAggregateParameterSet
            => await granny.ObtainAsync(
                ensemble, 
                primitives, 
                parameters, 
                parameters.EnsembleRepository, 
                parameters.UserId
                );
            /// <summary>
            /// Retrieves granny from ensemble if present; Otherwise, retrieves it from persistence or builds it, and adds it to the ensemble.
            /// </summary>
            /// <typeparam name="TGranny"></typeparam>
            /// <typeparam name="TParameterSet"></typeparam>
            /// <param name="granny"></param>
            /// <param name="ensemble"></param>
            /// <param name="parameters"></param>
            /// <param name="ensembleRepository"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            public async static Task<TGranny> ObtainAsync<TGranny, TParameterSet>(
                this IGranny<TGranny, TParameterSet> granny, 
                Ensemble ensemble,
                IPrimitiveSet primitives,
                TParameterSet parameters,
                IEnsembleRepository ensembleRepository,
                string userId
            ) 
            where TGranny : IGranny<TGranny, TParameterSet>
            where TParameterSet : IParameterSet
        {
            AssertionConcern.AssertArgumentNotNull(ensemble, nameof(ensemble));
            AssertionConcern.AssertArgumentNotNull(parameters, nameof(parameters));

            TGranny result = default;
            // if target is not in specified ensemble
            if (!granny.TryParse(ensemble, primitives, parameters, out TGranny ensembleParseResult))
            {
                // retrieve target from DB
                var grannyQueries = granny.GetQueries(primitives, parameters);
                Neuron previousGrannyNeuron = null;
                // loop through each grannyQuery
                foreach (var grannyQuery in grannyQueries)
                {
                    // if current grannyQuery requires retrievalResult
                    if (grannyQuery is IReceiver gqrcv)
                        gqrcv.SetRetrievalResult(previousGrannyNeuron);

                    // get ensemble based on parameters and previous granny neuron if it's assigned
                    var queryResult = await ensembleRepository.GetByQueryAsync(userId, grannyQuery.GetQuery());
                    // enrich ensemble
                    ensemble.AddReplaceItems(queryResult);
                    // if granny query is retriever
                    if (grannyQuery is IRetriever gqr)
                    {
                        // retrieve neuron
                        if ((previousGrannyNeuron = gqr.RetrieveNeuron(ensemble, primitives)) == null)
                            break;
                    }
                }

                // if target is in DB
                if (granny.TryParse(ensemble, primitives, parameters, out TGranny dbParseResult))
                {
                    result = dbParseResult;
                }
                // else if target is not in DB 
                else
                {
                    // build in ensemble
                    result = await granny.BuildAsync(ensemble, primitives, parameters);
                }
            }
            // if target was found in ensemble
            else if (ensembleParseResult != null)
                result = ensembleParseResult;

            return result;
        }

        internal static void TryParseCore<TGranny, TParameterSet>(
            this TGranny granny, 
            TParameterSet parameters, 
            Ensemble ensemble, 
            TGranny tempResult, 
            IEnumerable<Guid> selection, 
            LevelParser[] levelParsers, 
            System.Action<Neuron> grannySetter, 
            ref TGranny result
            )
            where TGranny : IGranny<TGranny, TParameterSet>
            where TParameterSet : IParameterSet
        {
            foreach (var levelParser in levelParsers)
                selection = levelParser.Evaluate(ensemble, selection);

            if (selection.Count() == 1 && ensemble.TryGetById(selection.Single(), out Neuron ensembleResult))
            {
                grannySetter(ensembleResult);
                result = tempResult;
            }
        }

        public static bool TryParseGranny<TParameters, TValue>(
            this TValue granny, 
            Ensemble ensemble,
            IPrimitiveSet primitives, 
            TParameters parameters,
            out IGranny result)
            where TValue : IGranny<TValue, TParameters>
            where TParameters : IParameterSet
        {
            result = null;

            if (granny.TryParse(ensemble, primitives, parameters, out TValue parseResult))
                result = parseResult;

            return result != null;
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
