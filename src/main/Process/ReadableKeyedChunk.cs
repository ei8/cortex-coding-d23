using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class ReadableKeyedChunk<T>(T key, IEnumerable<Neuron> contents) : IReadableChunk<T>
    {
        public T Key { get; } = key;

        public IEnumerable<Neuron> Contents { get; } = contents;
    }
}
