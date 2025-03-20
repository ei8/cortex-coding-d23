namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IInstanceValueParameterSet : IValueParameterSetCore, IDeductiveParameterSet
    {
        Neuron Class { get; }

        ValueMatchBy ValueMatchBy { get; }
    }
}
