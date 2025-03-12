namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IPropertyValueAssignmentParameterSet : IPropertyParameterSet, IDeductiveParameterSet
    {
        Neuron Value { get; }
    }
}
