namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    public interface IPropertyAssociationParameterSet : IReadParameterSet
    {
        Neuron Property { get; }

        Neuron Class { get; }
    }
}
