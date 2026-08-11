using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ei8.Cortex.Coding.d23.Process
{
    public static class WorkingMemoryExtensions
    {
        public static bool TryGetContents<TKey, TChunk, TResult>(
            this IWorkingMemory<TKey> workingMemory,
            TKey key,
            [NotNullWhen(true)]
            out TResult? result
        )
            where TKey : notnull
            where TChunk : IContentChunk<TResult>
            where TResult : IEnumerable<Neuron>
        {
            result = default;
            var boolResult = false;

            if (workingMemory.ContainsKey(key))
            {
                result = ((TChunk)workingMemory[key]).Contents;
                boolResult = true;
            }

            return boolResult;
        }
    }
}
