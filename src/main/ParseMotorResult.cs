using ei8.Cortex.Coding.Spiker;

namespace ei8.Cortex.Coding.d23
{
    public class ParseMotorResult(Neuron neuron, FireInfo fireInfo, object value)
    {
        public Neuron Neuron { get; } = neuron;
        public FireInfo FireInfo { get; } = fireInfo;
        public object Value { get; } = value;
    }
}
