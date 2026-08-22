using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public class ListChunk(IList<Neuron> content) : IListChunk
    {
        public ListChunk() : this(new List<Neuron>()) { }

        public IList<Neuron> Content { get; } = content;
    }
}
