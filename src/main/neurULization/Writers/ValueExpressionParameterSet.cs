namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class ValueExpressionParameterSet : ValueParameterSet, IValueExpressionParameterSet
    {
        public ValueExpressionParameterSet(
            Neuron value,
            Neuron @class,
            ValueMatchBy valueMatchBy
            ) : base(value, @class, valueMatchBy)
        {
        }
    }
}
