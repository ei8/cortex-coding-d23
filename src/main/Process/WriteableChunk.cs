using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class WriteableChunk(IList<Neuron> contents) : IWriteableChunk
    {
        public IList<Neuron> Contents { get; } = contents;
    }
}
