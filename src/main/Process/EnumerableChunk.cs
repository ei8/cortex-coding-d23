using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class EnumerableChunk<T>(IEnumerable<T> content) : ChunkBase<IEnumerable<T>>(content), IEnumerableChunk<T>
        where T : IChunk
    {
    }
}
