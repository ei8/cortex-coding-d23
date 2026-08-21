using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class EnumerableChunk(IEnumerable<Neuron> content) : IEnumerableChunk
    {
        public IEnumerable<Neuron> Content { get; } = content;
    }
}
