namespace ei8.Cortex.Coding.d23.Process
{
    public class NeuronChunk(Neuron value) : INeuronChunk
    {
        public Neuron Neuron { get; } = value;
    }
}
