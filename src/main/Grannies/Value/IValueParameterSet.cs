namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IValueParameterSet : IParameterSet
    {
        Neuron Value { get; }

        Neuron Class { get; }

        ValueMatchBy ValueMatchBy { get; }
    }
}
