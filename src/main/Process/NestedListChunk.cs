using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class NestedListChunk<T>(IList<IListChunk<T>> listChunks) : 
        ChunkBase<IList<IListChunk<T>>>(listChunks)
        where T : IChunk
    {
        public NestedListChunk() : this([]) { }
    }
}
