using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Library.Common;
using System;
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
                    retrievedGrannies
                )) != null)
                {
                    // get network based on parameters and previous granny neuron if it's assigned
                    var queryResult = await networkRepository.GetByQueryAsync(query);
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

        internal static bool TryParseAggregate<TReader, TParameterSet, TResult>(
            this TReader grannyReader,
            Func<TResult> resultConstructor,
            TParameterSet parameters,
            IEnumerable<IGreatGrannyProcess<TResult>> targets,
            Network network,
            IMirrorSet mirrors,
            out TResult result,
            bool setGrannyNeuronOnCompletion = true
        )
            where TResult : IGranny
            where TParameterSet : IDeductiveParameterSet
            where TReader : ILesserGrannyReader<TResult, TParameterSet>
        {
            result = default;
            bool bResult = false;

            var tempResult = resultConstructor();
            IGranny precedingGranny = null;
            var ts = targets.ToArray();

            if (grannyReader.TryCreateGreatGrannies(
                parameters,
                network,
                mirrors,
                out IEnumerable<IGreatGrannyInfo<TResult>> candidates
            ))
            {
                for (int i = 0; i < ts.Length; i++)
                {
                    var candidate = candidates.ElementAt(i);
                    if (
                        !(
                            ts[i].TryGetParameters(
                                precedingGranny,
                                candidate,
                                out IParameterSet executionParameters
                            ) &&
                            ts[i].TryExecute(
                                candidate,
                                network,
                                tempResult,
                                executionParameters,
                                out precedingGranny
                            )
                        )
                    )
                        break;
                    else if (i == ts.Length - 1)
                    {
                        if (setGrannyNeuronOnCompletion)
                            tempResult.Neuron = precedingGranny.Neuron;
                        result = tempResult;
                        bResult = true;
                    }
                }
            }

            return bResult;
        }
    }
}
