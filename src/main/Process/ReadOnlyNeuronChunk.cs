namespace ei8.Cortex.Coding.d23.Process
{
    public class ReadOnlyNeuronChunk(Neuron value) : 
        ReadOnlyChunkBase<Neuron>(value)
    {
        public Neuron Value => this.Content;
    }
}
