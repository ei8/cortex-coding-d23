using System.ComponentModel;

namespace ei8.Cortex.Coding.d23
{
    public class NeuronInfo
    (
        Neuron neuron,
        NeurotransmitterEffect effect,
        float strength
    )
    {
        public NeuronInfo(Neuron neuron) : this(neuron, NeurotransmitterEffect.Excite, 1)
        {
        }

        public Neuron Neuron { get; } = neuron;

        public NeurotransmitterEffect Effect { get; } = effect;

        public float Strength { get; } = strength;
    }
}
