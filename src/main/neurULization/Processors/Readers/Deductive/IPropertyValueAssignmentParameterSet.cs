namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IPropertyValueAssignmentParameterSet : IPropertyParameterSetCore, IDeductiveParameterSet
    {
        Neuron Value { get; }
    }
}
