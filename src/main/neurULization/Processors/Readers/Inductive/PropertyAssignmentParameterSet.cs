namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyAssignmentParameterSet : PropertyValueExpressionParameterSet, IPropertyAssignmentParameterSet
    {
        public PropertyAssignmentParameterSet(
            Neuron granny,
            Neuron property,
            Neuron @class
            ) : base(granny, @class)
        {
            Property = property;
        }

        public Neuron Property { get; }
    }
}