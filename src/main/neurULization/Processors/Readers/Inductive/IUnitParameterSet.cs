namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public interface IUnitParameterSet : IInductiveParameterSet
    {
        Neuron Value { get; }
        Neuron Type { get; }
    }
}
