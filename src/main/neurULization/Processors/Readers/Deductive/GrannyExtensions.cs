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
            INetworkRepository networkRepository,
            Network network,
            IList<IGranny> retrievedGrannies,
            string userId,
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
                    networkRepository, 
                    network, 
                    retrievedGrannies, 
                    userId
                )) != null)
                {
                    // get network based on parameters and previous granny neuron if it's assigned
                    var queryResult = await networkRepository.GetByQueryAsync(
                        query,
                        userId
                        );
                    // enrich network
                    network.AddReplaceItems(queryResult.Network);
                    // if granny query is retriever
                    if (grannyQuery is IRetriever gqr)
                    {
                        var retrievalResult = gqr.RetrieveGranny(
                            network,
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

        internal static bool AggregateTryParse<TAggregate>(
            this TAggregate aggregate,
            IEnumerable<IGreatGrannyInfo<TAggregate>> candidates,
            IEnumerable<IGreatGrannyProcess<TAggregate>> targets,
            Network network,
            out TAggregate result,
            bool setGrannyNeuronOnCompletion = true
        )
            where TAggregate : IGranny
        {
            result = default;

            IGranny precedingGranny = null;
            var ts = targets.ToArray();
            for (int i = 0; i < ts.Length; i++)
            {
                var candidate = candidates.ElementAt(i);
                if (ts[i].TryGetParameters(
                        precedingGranny,
                        candidate,
                        out IParameterSet parameters
                    ) && (
                        precedingGranny = ts[i].Execute(
                            candidate,
                            network,
                            aggregate,
                            parameters
                        )
                    ) == null)
                    break;
                else if (i == ts.Length - 1)
                {
                    if (setGrannyNeuronOnCompletion)
                        aggregate.Neuron = precedingGranny.Neuron;
                    result = aggregate;
                }
            }

            return result != null;
        }
    }
}
