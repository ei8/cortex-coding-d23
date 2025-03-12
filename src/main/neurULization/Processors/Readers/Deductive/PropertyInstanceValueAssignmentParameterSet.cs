namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyInstanceValueAssignmentParameterSet : PropertyInstanceValueExpressionParameterSet, IPropertyInstanceValueAssignmentParameterSet
    {
        public PropertyInstanceValueAssignmentParameterSet(
            Neuron property,
            Neuron value,
            Neuron @class,
            ValueMatchBy valueMatchBy
            ) : base(value, @class, valueMatchBy)
        {
            this.Property = property;
        }

        public Neuron Property { get; }
    }
}
