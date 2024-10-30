using ei8.Cortex.Coding.d23.Grannies;
using System;
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
            IGreatGrannyInfoSuperset<TResult> candidateSets, 
            IEnumerable<IGreatGrannyProcess<TResult>> targets, 
            Ensemble ensemble, 
            out TResult result
        )
            where TConcrete : TResult, new()
            where TResult : IGranny
        { 
            result = default;

            var tempResult = new TConcrete { Neuron = granny };

            int successCount = 0;
            IGranny precedingGranny = null;
            var candidateSetsList = candidateSets.Items.ToList();
            
            foreach (var candidateSet in candidateSetsList)
            {
                var candidateSetItemsList = candidateSet.Items.ToList();
                // TODO: string cacheId = AggregateParser.GetReadCacheId(
                //    ((IInductiveGreatGrannyInfo<TResult>)candidateSetItemsList[0]).Neuron,
                //    candidateSetItemsList[0].GetType().GenericTypeArguments[0]
                //);
                //AggregateParser.LogCacheRetrievalAttempt(cacheId);

                //if (this.cache.TryGetValue(cacheId, out IGranny gResult))
                //{
                //    AggregateParser.LogPrefixed("Retrieved!");
                //    result = (TResult)gResult;
                //}
                //else
                //{
                //    AggregateParser.LogPrefixed("Not found.");
                    foreach (var candidate in candidateSetItemsList)
                    {
                        AggregateParser.LogParseAttempt<TResult>(
                            candidate is IInductiveGreatGrannyInfo<TResult> ic ?
                                ic.Neuron :
                                (precedingGranny != null ? precedingGranny.Neuron : null),
                            candidateSetsList.Count(),
                            candidateSetsList.IndexOf(candidateSet),
                            candidateSetItemsList.Count(),
                            candidateSetItemsList.IndexOf(candidate)
                        );

                        if (AggregateParser.TryParse(
                            candidate,
                            targets, 
                            ensemble, 
                            tempResult, 
                            ref precedingGranny
                        ))
                        {
                            successCount++;
                            //this.cache.Add(
                            //    cacheId,
                            //    precedingGranny
                            //);
                            AggregateParser.LogParseSuccess(candidateSets.Count, successCount);
                            break;
                        }
                    }
                //}
            }

            if (successCount == candidateSets.Count)
                result = tempResult;

            return result != null;
        }

        private static bool TryParse<TConcrete, TResult>(
            IGreatGrannyInfo<TResult> candidate,
            IEnumerable<IGreatGrannyProcess<TResult>> targets, 
            Ensemble ensemble, 
            TConcrete tempResult, 
            ref IGranny precedingGranny
        )
            where TConcrete : TResult, new()
            where TResult : IGranny
        {
            bool result = false;
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
                    result = true;
                    break;
                }
            }

            return result;
        }

        private static string GetReadCacheId(Neuron granny, Type grannyType)
        {
            // TODO:1 cannot use index, must use parameters
            return $"{granny.Id}-{grannyType.FullName}";
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
        private static void LogParseAttempt<TResult>(
            Neuron granny, 
            int setCount, 
            int setIndex, 
            int itemCount, 
            int itemIndex
        ) where TResult : IGranny
        {
            var grannyId = granny != null ? granny.Id.ToString() : "[UNKNOWN]";
            var typeName = typeof(TResult).Name;
            Debug.WriteLine($"{GetPrefix()}Type: {typeof(TResult).Name}; GrannyId: {grannyId}: Expected: {setCount}; SetIndex: {setIndex}; ItemCount: {itemCount}; Index: {itemIndex}");
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
