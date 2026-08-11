using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class WriteableKeyedChunk<T>(T key, IList<Neuron> contents) : IWriteableChunk<T>
    {
        public T Key { get; } = key;

        public IList<Neuron> Contents { get; } = contents;
    }
}
