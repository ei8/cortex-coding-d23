namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    public interface IUnitParameterSet : IReadParameterSet
    {
        Neuron Value { get; }
        Neuron Type { get; }
    }
}
