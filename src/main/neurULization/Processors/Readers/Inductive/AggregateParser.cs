using ei8.Cortex.Coding.d23.Grannies;
using Newtonsoft.Json;
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
        private const string prefixPattern = "Reader.TryParse";

        private readonly IDictionary<string, IGranny> cache;
        private readonly IList<Tuple<IGranny, IGranny>> updatedGrannyCandidates;
        private readonly List<string> skipIds;

        public AggregateParser()
        {
            this.cache = new Dictionary<string, IGranny>();
            this.updatedGrannyCandidates = new List<Tuple<IGranny, IGranny>>();
            this.skipIds = new List<string>();
        }

        public bool TryParse<TConcrete, TAggregate>(
            Neuron granny,
            IGreatGrannyInfoSuperset<TAggregate> candidateSets,
            Network network,
            out TAggregate aggregate
        )
            where TConcrete : TAggregate, new()
            where TAggregate : IGranny
        {
            aggregate = default;

            var tempAggregate = new TConcrete { Neuron = granny };

            int successCount = 0;
            IGranny precedingGranny = null;
            var candidateSetsList = candidateSets.Items.ToList();

            AggregateParser.LogParse<TAggregate>(granny);

            foreach (var candidateSet in candidateSetsList)
            {
                AggregateParser.LogProcessStart(
                    $"Analyzing candidate set... " +
                    $"({candidateSetsList.IndexOf(candidateSet) + 1}/{candidateSetsList.Count()})", 
                    1, 
                    true
                );
                var candidateSetItemsList = candidateSet.Items.ToList();
                foreach (var candidate in candidateSetItemsList)
                {
                    AggregateParser.LogCandidateDetails<TAggregate>(
                        candidate,
                        precedingGranny,
                        candidateSetItemsList
                    );

                    IGranny tempPrecedingGranny = null;

                    // GetParameters before calling TryParse
                    if (candidateSet.Target.TryGetParameters(
                        precedingGranny,
                        candidate,
                        out IParameterSet parameters
                    ))
                    {
                        AggregateParser.LogCandidateLine($"Obtaining parameters... SUCCESS");
                        AggregateParser.LogPropertyName(parameters);

                        // ...formulate cacheId
                        bool createCacheIdResult = AggregateParser.TryCreateCacheId(candidate, precedingGranny, parameters, out string cacheId);

                        var createCacheIdResultString = createCacheIdResult ? $" SUCCESS - {cacheId}" : "FAIL";

                        AggregateParser.LogCandidateLine($"Creating cacheId...{createCacheIdResultString}");

                        var retrievalResult = false;
                        var retrievalDetails = new StringBuilder();
                        var skipping = this.skipIds.Contains(cacheId);
                        if (!skipping)
                        {
                            if (createCacheIdResult)
                                AggregateParser.LogProcessStart("Retrieving from cache...", 3);

                            // ... check cache
                            if (retrievalResult = createCacheIdResult && this.cache.TryGetValue(cacheId, out tempPrecedingGranny))
                            {
                                if (!this.updatedGrannyCandidates.Any(ugc => ugc.Item1 == tempPrecedingGranny && ugc.Item2 == (IGranny)tempAggregate))
                                {
                                    AggregateParser.LogDetails("Updating aggregate...", retrievalDetails, 4);
                                    candidateSet.Target.UpdateAggregate(candidate, tempPrecedingGranny, tempAggregate);
                                    this.updatedGrannyCandidates.Add(Tuple.Create(tempPrecedingGranny,
                                        (IGranny)tempAggregate));
                                }
                                precedingGranny = tempPrecedingGranny;
                                successCount++;
                            }
                        }
                        else
                            AggregateParser.LogProcessStart("Skipped...", 3);

                        if (createCacheIdResult)
                            AggregateParser.LogResultInline(retrievalResult, candidateSets.Count, successCount, retrievalDetails);

                        if (retrievalResult)
                        {
                            AggregateParser.LogCandidateSetProgress(retrievalResult, candidateSets.Count, successCount, null);
                            break;
                        }
                        else if (skipping)
                        {
                            AggregateParser.LogCandidateSetProgress(retrievalResult, candidateSets.Count, successCount, null);
                            continue;
                        }
                        else
                        {
                            var parseResult = (tempPrecedingGranny = candidateSet.Target.Execute(
                                candidate,
                                network,
                                tempAggregate,
                                parameters
                            )) != null;
                            var parseDetails = new StringBuilder();

                            if (parseResult)
                            {
                                AggregateParser.LogDetails($"Parse... SUCCESS", parseDetails, 3);
                                this.updatedGrannyCandidates.Add(Tuple.Create(tempPrecedingGranny,
                                    (IGranny)tempAggregate));
                                precedingGranny = tempPrecedingGranny;
                                successCount++;

                                if (createCacheIdResult)
                                {
                                    this.cache.Add(
                                       cacheId,
                                       precedingGranny
                                    );
                                    AggregateParser.LogDetails($"Added to cache.", parseDetails, 4);
                                }
                                else
                                    AggregateParser.LogDetails($"No cacheId, unable to cache.", parseDetails, 4);
                            }
                            else
                            {
                                AggregateParser.LogDetails($"Parse... FAIL", parseDetails, 3);
                                if (createCacheIdResult)
                                {
                                    skipIds.Add(cacheId);
                                    AggregateParser.LogDetails($"Added to skip list.", parseDetails, 4);
                                }
                                else
                                    AggregateParser.LogDetails($"No cacheId, unable to add to skip list.", parseDetails, 4);
                            }

                            AggregateParser.LogCandidateSetProgress(parseResult, candidateSets.Count, successCount, parseDetails);

                            if (parseResult)
                                break;
                        }
                    }
                    else
                    {
                        AggregateParser.LogCandidateLine($"Obtaining parameters... FAIL");
                        AggregateParser.LogCandidateSetProgress(false, candidateSets.Count, successCount, null);
                    }
                }

                if (successCount == candidateSets.Count)
                    aggregate = tempAggregate;
            }

            return aggregate != null;
        }

        private static bool TryCreateCacheId<T>(
            IGreatGrannyInfo<T> candidate, 
            IGranny precedingGranny, 
            IParameterSet parameters, 
            out string result
        )
        {
            var candidateId = candidate is IInductiveGreatGrannyInfo<T> ic ?
               ic.Neuron.Id.ToString() :
               null;

            result = string.Empty;

            if (!string.IsNullOrWhiteSpace(candidateId))
            {
                var resultAnon = new
                {
                    CandidateId = candidateId,
                    CandidateType = candidate.GetType().GenericTypeArguments[0].FullName,
                    Parameters = parameters != null ? JsonConvert.SerializeObject(parameters, Formatting.None) : null,
                    PrecedingId = precedingGranny != null ? precedingGranny.Neuron.Id.ToString() : null,
                    PrecedingType = precedingGranny != null ? precedingGranny.GetType().FullName : null
                };

                result = JsonConvert.SerializeObject(resultAnon, Formatting.None);
            }

            return !string.IsNullOrWhiteSpace(result);
        }

        [Conditional("PARSELOG")]
        private static void LogProcessStart(string processName, int indent = 0, bool writeLine = false)
        {
            var logMessage = $"{AggregateParser.GetPrefix(AggregateParser.Counter, indent)}{processName}";
            
            if (writeLine)
                Debug.WriteLine(logMessage);
            else
                Debug.Write(logMessage);
        }

        [Conditional("PARSELOG")]
        private static void LogDetails(string value, StringBuilder stringBuilder, int indent = 1)
        {
            value = $"{AggregateParser.GetPrefix(AggregateParser.Counter, indent)}{value}";
            stringBuilder.AppendLine(value);
        }

        [Conditional("PARSELOG")]
        private static void LogResultInline(bool success, int expectedGreatGrannyCount, int successCount, StringBuilder details)
        {
            var counts = string.Empty;

            Debug.WriteLine($" {(success ? "SUCCESS" : "FAIL")}{counts}");

            if (details != null)
                Debug.Write(details.ToString());
        }

        [Conditional("PARSELOG")]
        private static void LogCandidateSetProgress(bool success, int expectedGreatGrannyCount, int successCount, StringBuilder details)
        {
            var indent = 2;

            if (details != null)
                Debug.Write(details.ToString());

            Debug.WriteLine(
                $"{AggregateParser.GetPrefix(AggregateParser.Counter, indent)}" +
                $"SUCCESS: {successCount} ({(success ? "+1, " : string.Empty)}" +
                $"{100 * successCount/expectedGreatGrannyCount}%)"
            );
        }

        [Conditional("PARSELOG")]
        private static void LogParse<TResult>(
            Neuron granny
        ) where TResult : IGranny
        {
            Debug.WriteLine(
                $"{AggregateParser.GetPrefix(AggregateParser.Counter)}Parsing Granny... Type: {typeof(TResult).Name}; " +
                $"Id: {granny.Id}"
            );
        }

        [Conditional("PARSELOG")]
        private static void LogCandidateDetails<TResult>(
            IGreatGrannyInfo<TResult> candidate,
            IGranny precedingGranny,
            IList<IGreatGrannyInfo<TResult>> candidateSetItemsList
        ) where TResult : IGranny
        {
            var candidateId = candidate is IInductiveGreatGrannyInfo<TResult> ic ?
                ic.Neuron.Id.ToString() :
                "N/A";
            var precedingGrannyType = precedingGranny != null ?
                precedingGranny.GetType().FullName :
                "N/A";
            var precedingGrannyId = precedingGranny != null ? 
                precedingGranny.Neuron.Id.ToString() : 
                "N/A";
            var itemIndex = candidateSetItemsList.IndexOf(candidate);
            var itemCount = candidateSetItemsList.Count();

            Debug.WriteLine(
                $"{AggregateParser.GetPrefix(AggregateParser.Counter, 2)}Analyzing candidate... ({itemIndex + 1}/{itemCount}) - Type: {candidate.GetType().GenericTypeArguments[0].FullName}; " +
                $"CandidateId: {candidateId}; " +
                $"PrecedingType: {precedingGrannyType}; " +
                $"PrecedingId: {precedingGrannyId} "
            );
        }

        [Conditional("PARSELOG")]
        private static void LogPropertyName(IParameterSet parameters)
        {
            if (parameters is Processors.Readers.IPropertyParameterSet prop)
                LogCandidateLine($"Property Name: {prop.Property.Tag}");
        }

        [Conditional("PARSELOG")]
        private static void LogCandidateLine(string value)
        {
            Debug.WriteLine($"{AggregateParser.GetPrefix(AggregateParser.Counter, 3)}{value}");
        }

        internal static string GetPrefix(Func<int, int> counter, int indent = 0)
        {
            var prefix = $"{new StringBuilder().Insert(0, "|--", counter(indent))}";
            return prefix;
        }

        private static int Counter(int indent)
        {
            var st = new StackTrace().ToString();
            var rootIndentation = 1;
            var tryParseCount = Regex.Matches(st, AggregateParser.prefixPattern).Count;
            tryParseCount += tryParseCount > 1 ? ((tryParseCount -  1) *  2) : 0;
            return tryParseCount + indent - rootIndentation;
        }
    }
}
