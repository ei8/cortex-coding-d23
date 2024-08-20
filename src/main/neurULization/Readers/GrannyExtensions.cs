using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

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
            foreach (var candidate in greatGrannyCandidates)
            {
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
    }
}
