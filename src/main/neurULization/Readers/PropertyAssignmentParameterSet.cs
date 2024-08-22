namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    public class PropertyAssignmentParameterSet : PropertyValueExpressionParameterSet, IPropertyAssignmentParameterSet
    {
        public PropertyAssignmentParameterSet(
            Neuron granny,
            Neuron property,
            Neuron @class
            ) : base(granny, @class)
        {
            this.Property = property;
        }

        public Neuron Property { get; }
    }
}