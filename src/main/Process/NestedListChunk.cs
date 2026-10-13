using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class NestedListChunk<T>(IList<IListChunk<T>> listChunks) : 
        INestedListChunk<IListChunk<T>, T>
        where T : IChunk
    {
        public NestedListChunk() : this([]) { }

        public IList<IListChunk<T>> Lists { get; } = listChunks;
    }
}
