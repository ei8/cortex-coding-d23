namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    public interface IPropertyAssignmentParameterSet : IReadParameterSet
    {
        Neuron Property { get; }

        Neuron Class { get; }
    }
}
