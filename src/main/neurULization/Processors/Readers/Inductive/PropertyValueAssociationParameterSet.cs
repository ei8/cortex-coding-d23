using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyValueAssociationParameterSet : IPropertyValueAssociationParameterSet
    {
        public static PropertyValueAssociationParameterSet CreateWithoutGranny(
            Neuron property
        ) => new PropertyValueAssociationParameterSet(null, property);

        public static PropertyValueAssociationParameterSet CreateWithGranny(
            Neuron granny,
            Neuron property
        )
        {
            AssertionConcern.AssertArgumentNotNull(granny, nameof(granny));

            return new PropertyValueAssociationParameterSet(granny, property);
        }

        private PropertyValueAssociationParameterSet(
            Neuron granny,
            Neuron property
        )
        {
            AssertionConcern.AssertArgumentNotNull(property, nameof(property));

            this.Granny = granny;
            this.Property = property;
        }

        public Neuron Granny { get; }

        public Neuron Property { get; }
    }
}
