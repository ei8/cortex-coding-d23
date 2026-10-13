using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class EnumerableChunk<T>(IEnumerable<T> items) : IEnumerableChunk<T>
        where T : IChunk
    {
        public IEnumerable<T> Items { get; } = items;
    }
}
