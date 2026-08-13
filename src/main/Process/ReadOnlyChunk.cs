using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class ReadOnlyChunk(IEnumerable<Neuron> contents) : IReadOnlyChunk
    {
        public IEnumerable<Neuron> Contents { get; } = contents;
    }
}
