using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public static class GrannyExtensions
    {
        public static async Task<bool> Process(
            this IEnumerable<IGrannyQuery> grannyQueries,
            IEnsembleRepository ensembleRepository,
            Ensemble ensemble,
            IList<IGranny> retrievedGrannies,
            string userId,
            string cortexLibraryOutBaseUrl, 
            int queryResultLimit,
            bool breakBeforeLastGetQuery = false
            )
        {
            var result = false;
            // loop through each grannyQuery
            var grannyQueriesArr = grannyQueries.ToArray();
            for (int i = 0; i < grannyQueriesArr.Length; i++)
            {
                var grannyQuery = grannyQueriesArr[i];
                var nextGrannyQuery = i < grannyQueriesArr.Length - 1 ?
                    grannyQueriesArr[i + 1] :
                    null;
                // if last is supposed to be skipped
                if (breakBeforeLastGetQuery && grannyQueries.Last() == grannyQuery)
                {
                    // indicate success then break
                    result = true;
                    break;
                }

                NeuronQuery query = null;

                // if query is obtained successfully
                if ((query = await grannyQuery.GetQuery(
                    ensembleRepository, 
                    ensemble, 
                    retrievedGrannies, 
                    userId,
                    cortexLibraryOutBaseUrl,
                    queryResultLimit
                )) != null)
                {
                    // get ensemble based on parameters and previous granny neuron if it's assigned
                    var queryResult = await ensembleRepository.GetByQueryAsync(
                        userId,
                        query,
                        cortexLibraryOutBaseUrl,
                        queryResultLimit
                        );
                    // enrich ensemble
                    ensemble.AddReplaceItems(queryResult);
                    // if granny query is retriever
                    if (grannyQuery is IRetriever gqr)
                    {
                        var retrievalResult = gqr.RetrieveGranny(
                            ensemble,
                            retrievedGrannies.AsEnumerable()
                            );
                        retrievedGrannies.Add(retrievalResult);
                        // if retrieval fails and this is not the last query
                        // and next grannyQuery cannot continue without current result
                        if (
                                retrievalResult == null &&
                                grannyQueries.Last() != grannyQuery &&

                                    nextGrannyQuery != null &&
                                    nextGrannyQuery.RequiresPrecedingGrannyQueryResult

                            )
                            // break with a failure indication
                            break;
                    }
                }
                else
                    // break with a failure indication
                    break;

                // if this is the last granny
                if (grannyQueries.Last() == grannyQuery)
                {
                    // indicate success
                    result = true;
                }
            }
            return result;
        }

        internal static bool AggregateTryParse<TResult>(
            this TResult tempResult,
            IEnumerable<IGreatGrannyInfo<TResult>> processes,
            IEnumerable<IGreatGrannyProcess<TResult>> targets,
            Ensemble ensemble,
            out TResult result,
            bool setGrannyNeuronOnCompletion = true
        )
            where TResult : IGranny
        {
            result = default;

            IGranny precedingGranny = null;
            var ts = targets.ToArray();
            for (int i = 0; i < ts.Length; i++)
            {
                if ((precedingGranny = ts[i].Execute(
                    processes.ElementAt(i),
                    ensemble,
                    precedingGranny,
                    tempResult
                    )) == null)
                    break;
                else if (i == ts.Length - 1)
                {
                    if (setGrannyNeuronOnCompletion)
                        tempResult.Neuron = precedingGranny.Neuron;
                    result = tempResult;
                }
            }

            return result != null;
        }
    }
}
