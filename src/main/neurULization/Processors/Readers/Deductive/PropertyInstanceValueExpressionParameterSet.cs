namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyInstanceValueExpressionParameterSet : InstanceValueExpressionParameterSet, IPropertyInstanceValueExpressionParameterSet
    {
        public PropertyInstanceValueExpressionParameterSet(
            Neuron value,
            Neuron @class,
            ValueMatchBy valueMatchBy
            ) : base(value, @class, valueMatchBy)
        {            
        }
    }
}
