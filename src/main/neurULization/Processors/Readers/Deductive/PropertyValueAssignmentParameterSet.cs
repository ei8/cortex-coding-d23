namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyValueAssignmentParameterSet : PropertyValueExpressionParameterSet, IPropertyValueAssignmentParameterSet
    {
        public PropertyValueAssignmentParameterSet(
            Neuron property,
            Neuron value
            ) : base(value)
        {
            this.Property = property;
        }

        public Neuron Property { get; }
    }
}