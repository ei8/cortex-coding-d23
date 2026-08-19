using System.ComponentModel;

namespace ei8.Cortex.Coding.d23
{
    public class NeuronInfo
    (
        Neuron neuron,
        float strength,
        NeurotransmitterEffect effect
    )
    {
        public NeuronInfo(Neuron neuron) : this(neuron, 1, NeurotransmitterEffect.Excite)
        {
        }

        public Neuron Neuron { get; } = neuron;

        public float Strength { get; } = strength;

        public NeurotransmitterEffect Effect { get; } = effect;
    }
}
