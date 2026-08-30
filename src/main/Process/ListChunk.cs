using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class ListChunk<T>(IList<T> content) : ChunkBase<IList<T>>(content), IListChunk<T>
        where T : IChunk
    {
        public ListChunk() : this([]) { }
    }
}
