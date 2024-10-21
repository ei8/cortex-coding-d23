using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class AggregateParser : IAggregateParser
    {
        private readonly IDictionary<string, IGranny> cache;
        public AggregateParser()
        {
            this.cache = new Dictionary<string, IGranny>();
        }

        public bool TryParse<TConcrete, TResult>(
            Neuron granny, 
            IEnumerable<IGreatGrannyInfo<TResult>> candidates, 
            IEnumerable<IGreatGrannyProcess<TResult>> targets, 
            Ensemble ensemble, 
            int expectedGreatGrannyCount, 
            out TResult result
        )
            where TConcrete : TResult, new()
            where TResult : IGranny
        { 
            result = default;

            var tempResult = new TConcrete();
            tempResult.Neuron = granny;

            int successCount = 0;
            IGranny precedingGranny = null;
            var candidatesList = candidates.ToList();

            foreach (var candidate in candidatesList)
            {
                var index = candidatesList.IndexOf(candidate);

                AggregateParser.LogParseAttempt<TResult>(granny, expectedGreatGrannyCount, index);

                var cacheId = AggregateParser.GetReadCacheId<TResult>(granny, index);

                //AggregateParser.LogCacheRetrievalAttempt(cacheId);
                //if (this.cache.TryGetValue(cacheId, out IGranny gResult))
                //{
                //    AggregateParser.LogPrefixed("Retrieved!");
                //    result = (TResult)gResult;
                //}
                //else
                //{
                //    AggregateParser.LogPrefixed("Not found.");

                    foreach (var target in targets)
                    {
                        var tempPrecedingGranny = default(IGranny);
                        if ((tempPrecedingGranny = target.Execute(
                            candidate,
                            ensemble,
                            precedingGranny,
                            tempResult
                            )) != null)
                        {
                            precedingGranny = tempPrecedingGranny;
                            successCount++;
                            AggregateParser.LogParseSuccess(expectedGreatGrannyCount, successCount);
                            break;
                        }
                    }
                    if (successCount == expectedGreatGrannyCount)
                    {
                        result = tempResult;
                        //this.cache.Add(
                        //    cacheId,
                        //    result
                        //);
                        break;
                    }
                //}
            }

            return result != null;
        }

        private static string GetReadCacheId<TResult>(Neuron granny, int index) where TResult : IGranny
        {
            // TODO:1 cannot use index, must use parameters
            return $"{granny.Id}-{typeof(TResult).FullName}-{index}";
        }

        [Conditional("DEBUG")]
        private static void LogCacheRetrievalAttempt(string cacheId)
        {
            Debug.WriteLine($"{GetPrefix()}Retrieving cache... Id: '{cacheId}'");
        }

        [Conditional("DEBUG")]
        private static void LogPrefixed(string value)
        {
            Debug.WriteLine($"{GetPrefix()}{value}");
        }

        [Conditional("DEBUG")]
        private static void LogParseSuccess(int expectedGreatGrannyCount, int successCount)
        {
            Debug.WriteLine($"{GetPrefix()}|--PARSED! ({successCount}/{expectedGreatGrannyCount})");
        }

        [Conditional("DEBUG")]
        private static void LogParseAttempt<TResult>(Neuron granny, int expectedGreatGrannyCount, int index) where TResult : IGranny
        {
            Debug.WriteLine($"{GetPrefix()}Type: {typeof(TResult).Name}; GrannyId: {granny.Id}: Expected: {expectedGreatGrannyCount}; Index: {index}");
        }

        private static string GetPrefix()
        {
            var st = new StackTrace();
            var count = Regex.Matches(st.ToString(), "Reader." + nameof(IAggregateParser.TryParse)).Count;
            var prefix = $"{new StringBuilder().Insert(0, "|--", count - 1)}";
            return prefix;
        }
    }
}
