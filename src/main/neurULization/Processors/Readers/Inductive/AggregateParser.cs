﻿using ei8.Cortex.Coding.d23.Grannies;
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
                foreach (var candidate in candidateSetItemsList)
                {
                    string cacheId = AggregateParser.GetReadCacheId(candidate);
                    AggregateParser.LogCacheRetrievalAttempt(cacheId);

                    if (this.cache.TryGetValue(cacheId, out IGranny gResult))
                    {
                        precedingGranny = gResult;
                        // TODO: sequence contains more than one element
                        // is the aggregate getting over-updated?
                        // how can we determine whether an update is still necessary?
                        candidateSet.Target.UpdateAggregate(candidate, precedingGranny, tempResult);
                        successCount++;
                        AggregateParser.LogSuccess("retrieved", candidateSets.Count, successCount);
                        break;
                    }
                    else
                    {
                        AggregateParser.LogPrefixed("|--Not found.");
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
                            candidateSet.Target,
                            ensemble, 
                            tempResult, 
                            ref precedingGranny
                        ))
                        {
                            successCount++;
                            if (!string.IsNullOrWhiteSpace(cacheId))
                                this.cache.Add(
                                   cacheId,
                                   precedingGranny
                                );
                            AggregateParser.LogSuccess("parsed", candidateSets.Count, successCount);
                            break;
                        }
                    }
                }
            }

            if (successCount == candidateSets.Count)
                result = tempResult;

            return result != null;
        }

        private static bool TryParse<TConcrete, TResult>(
            IGreatGrannyInfo<TResult> candidate,
            IGreatGrannyProcess<TResult> target, 
            Ensemble ensemble, 
            TConcrete tempResult, 
            ref IGranny precedingGranny
        )
            where TConcrete : TResult, new()
            where TResult : IGranny
        {
            bool result = false;
            IGranny tempPrecedingGranny;
            if ((tempPrecedingGranny = target.Execute(
                candidate,
                ensemble,
                precedingGranny,
                tempResult
                )) != null)
            {
                precedingGranny = tempPrecedingGranny;
                result = true;
            }
            
            return result;
        }

        private static string GetReadCacheId<TResult>(IGreatGrannyInfo<TResult> candidateSetItem)
        {
            string result = string.Empty;

            var granny = candidateSetItem is IInductiveGreatGrannyInfo<TResult> inductive ? 
                inductive.Neuron : 
                null;
            
            if (granny != null)
            {
                var grannyType = candidateSetItem.GetType().GenericTypeArguments[0];
                result = $"{granny.Id}-{grannyType.FullName}";
            }
            
            return result;
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
        private static void LogSuccess(string message, int expectedGreatGrannyCount, int successCount)
        {
            Debug.WriteLine($"{GetPrefix()}|--{message.ToUpper()}! ({successCount}/{expectedGreatGrannyCount})");
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
