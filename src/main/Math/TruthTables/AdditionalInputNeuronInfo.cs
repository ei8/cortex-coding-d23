namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public class AdditionalInputNeuronInfo
    (
        AdditionalInputNeuronInfo.TypeValue type,
        params Neuron[] neurons
    )
    {
        public enum TypeValue
        {
            Or,
            And,
            Manual
        }

        public AdditionalInputNeuronInfo(params Neuron[] neurons) : this(TypeValue.And, neurons) { }

        public AdditionalInputNeuronInfo.TypeValue Type { get; } = type;
        public Neuron[] Neurons { get; } = neurons;
    }
}
