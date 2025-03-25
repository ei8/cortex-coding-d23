namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyValueAssignmentParameterSet : PropertyValueExpressionParameterSet, IPropertyValueAssignmentParameterSet
    {
        public PropertyValueAssignmentParameterSet(
            Neuron granny,
            Neuron property
            ) : base(granny)
        {
            Property = property;
        }

        public Neuron Property { get; }
    }
}