namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyInstanceValueAssociationParameterSet : PropertyInstanceValueAssignmentParameterSet, IPropertyInstanceValueAssociationParameterSet
    {
        public PropertyInstanceValueAssociationParameterSet(
            Neuron property,
            Neuron value,
            Neuron @class,
            ValueMatchBy valueMatchBy
            ) : base(property, value, @class, valueMatchBy)
        {
        }
    }
}
