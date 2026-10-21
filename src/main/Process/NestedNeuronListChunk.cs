using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class NestedNeuronListChunk(IList<IListChunk<INeuronChunk>> listChunks) : 
        INestedNeuronListChunk<IListChunk<INeuronChunk>>
    {
        public NestedNeuronListChunk() : this([]) { }

        public IList<IListChunk<INeuronChunk>> Lists { get; } = listChunks;
    }
}
