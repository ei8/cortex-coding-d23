namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public interface IPropertyAssociationParameterSet : IWriteParameterSet
    {
        Neuron Property { get; }

        Neuron Value { get; }

        Neuron Class { get; }

        ValueMatchBy ValueMatchBy { get; }
    }
}
