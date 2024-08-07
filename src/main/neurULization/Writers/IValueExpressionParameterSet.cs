namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public interface IValueExpressionParameterSet : IWriteParameterSet
    {
        Neuron Value { get; }

        Neuron Class { get; }

        ValueMatchBy ValueMatchBy { get; }
    }
}
