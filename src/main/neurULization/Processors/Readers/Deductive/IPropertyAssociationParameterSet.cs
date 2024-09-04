namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IPropertyAssociationParameterSet : IDeductiveParameterSet
    {
        Neuron Property { get; }

        Neuron Value { get; }

        Neuron Class { get; }

        ValueMatchBy ValueMatchBy { get; }
    }
}
