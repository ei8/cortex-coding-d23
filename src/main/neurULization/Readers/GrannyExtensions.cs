using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    internal static class GrannyExtensions
    {
        internal static bool AggregateTryParse<TResult>(
            this TResult tempResult,
            Neuron granny,
            IEnumerable<IGreatGrannyInfo<TResult>> greatGrannyCandidates,
            IEnumerable<IGreatGrannyProcess<TResult>> targets,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            int expectedGreatGrannyCount,
            out TResult result
        )
            where TResult : IGranny
        {
            result = default;

            tempResult.Neuron = granny;

            int successCount = 0;
            IGranny precedingGranny = null;
            var greatGrannyCandidatesList = greatGrannyCandidates.ToList();

            foreach (var candidate in greatGrannyCandidatesList)
            {
                GrannyExtensions.LogParseAttempt(granny, expectedGreatGrannyCount, greatGrannyCandidatesList, candidate);

                foreach (var target in targets)
                {
                    var tempPrecedingGranny = default(IGranny);
                    if ((tempPrecedingGranny = target.Execute(
                        candidate,
                        ensemble,
                        options,
                        precedingGranny,
                        tempResult
                        )) != null)
                    {
                        precedingGranny = tempPrecedingGranny;
                        successCount++;
                        GrannyExtensions.LogParseSuccess(expectedGreatGrannyCount, successCount);
                        break;
                    }
                }
                if (successCount == expectedGreatGrannyCount)
                {
                    result = tempResult;
                    break;
                }
            }

            return result != null;
        }

        [Conditional("DEBUG")]
        private static void LogParseSuccess(int expectedGreatGrannyCount, int successCount)
        {
            Debug.WriteLine($"{GrannyExtensions.GetPrefix()}|--PARSED! ({successCount}/{expectedGreatGrannyCount})");
        }

        [Conditional("DEBUG")]
        private static void LogParseAttempt<TResult>(Neuron granny, int expectedGreatGrannyCount, List<IGreatGrannyInfo<TResult>> greatGrannyCandidatesList, IGreatGrannyInfo<TResult> candidate) where TResult : IGranny
        {
            Debug.WriteLine($"{GrannyExtensions.GetPrefix()}Type: {typeof(TResult).Name}; GrannyId: {granny.Id}: Expected: {expectedGreatGrannyCount}; Index: {greatGrannyCandidatesList.IndexOf(candidate)}");
        }

        private static string GetPrefix()
        {
            var st = new StackTrace();
            var count = Regex.Matches(st.ToString(), "AggregateTryParse").Count;
            var prefix = $"{new StringBuilder().Insert(0, "|--", count - 1)}";
            return prefix;
        }
    }
}
