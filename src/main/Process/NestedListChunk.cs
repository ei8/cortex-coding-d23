using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class NestedListChunk(IList<IListChunk> listChunks) : 
        ReadOnlyChunkBase<IList<IListChunk>>(listChunks)
    {
        public NestedListChunk() : this([]) { }
    }
}
