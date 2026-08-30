using System;

namespace ei8.Cortex.Coding.d23.Process
{
    public class NeuronChunk(Neuron value) :
        ChunkBase<Neuron>(value)
    {
        public Neuron Value => this.Content;
    }
}
