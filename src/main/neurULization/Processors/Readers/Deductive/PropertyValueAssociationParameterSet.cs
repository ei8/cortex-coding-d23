namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyValueAssociationParameterSet : PropertyValueAssignmentParameterSet, IPropertyValueAssociationParameterSet
    {
        public PropertyValueAssociationParameterSet(
            Neuron property,
            Neuron value
            ) : base(property, value)
        {
        }
    }
}
