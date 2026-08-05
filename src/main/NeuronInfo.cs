namespace ei8.Cortex.Coding.d23
{
    public class NeuronInfo
    {
        public NeuronInfo(Neuron neuron) : this(neuron, 1, NeurotransmitterEffect.Excite)
        {
        }

        public NeuronInfo(
            Neuron neuron,
            float strength,
            NeurotransmitterEffect effect
        )
        {
            this.Neuron = neuron;
            this.Strength = strength;
            this.Effect = effect;
        }

        public Neuron Neuron { get; }

        public float Strength { get; }

        public NeurotransmitterEffect Effect { get; }
    }
}
