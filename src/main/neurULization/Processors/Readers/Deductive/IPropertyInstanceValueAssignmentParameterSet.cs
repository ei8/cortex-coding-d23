namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IPropertyInstanceValueAssignmentParameterSet : IPropertyParameterSetCore, IDeductiveParameterSet
    {
        Neuron Value { get; }

        Neuron Class { get; }

        ValueMatchBy ValueMatchBy { get; }
    }
}
