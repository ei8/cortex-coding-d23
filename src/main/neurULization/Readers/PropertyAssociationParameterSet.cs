namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    public class PropertyAssociationParameterSet : PropertyAssignmentParameterSet, IPropertyAssociationParameterSet
    {
        public PropertyAssociationParameterSet(
            Neuron granny,
            Neuron property,
            Neuron @class
            ) : base(granny, property, @class)
        {
        }
    }
}
