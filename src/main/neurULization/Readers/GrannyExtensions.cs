using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    internal static class GrannyExtensions
    {
        internal static bool AggregateTryParse<TResult>(
            this TResult tempResult,
            IReadParameterSet parameters,
            IEnumerable<IGreatGrannyInfo<TResult>> greatGrannyCandidateProcesses,
            IEnumerable<IGreatGrannyReadProcess<TResult>> targets,
            Ensemble ensemble,
            Id23neurULizerOptions options,
            int expectedGreatGrannyCount,
            out TResult result
        )
            where TResult : IGranny
        {
            result = default;

            tempResult.Neuron = parameters.Granny;

            int successCount = 0;
            foreach (var candidateProcess in greatGrannyCandidateProcesses)
            {
                foreach (var target in targets)
                {
                    if (target.Execute(candidateProcess, ensemble, options, tempResult) != null)
                    {
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
