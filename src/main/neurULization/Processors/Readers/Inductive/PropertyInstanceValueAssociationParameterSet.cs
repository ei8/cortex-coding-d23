using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyInstanceValueAssociationParameterSet : IPropertyInstanceValueAssociationParameterSet
    {
        public static PropertyInstanceValueAssociationParameterSet CreateWithoutGranny(
            Neuron property,
            Neuron @class
        ) => new PropertyInstanceValueAssociationParameterSet(null, property, @class);

        public static PropertyInstanceValueAssociationParameterSet CreateWithGranny(
            Neuron granny,
            Neuron property,
            Neuron @class
        )
        {
            AssertionConcern.AssertArgumentNotNull(granny, nameof(granny));

            return new PropertyInstanceValueAssociationParameterSet(granny, property, @class);
        }

        private PropertyInstanceValueAssociationParameterSet(
            Neuron granny,
            Neuron property,
            Neuron @class
            )
        {
            AssertionConcern.AssertArgumentNotNull(property, nameof(property));
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));

            this.Granny = granny;
            this.Property = property;
            this.Class = @class;
        }

        public Neuron Granny { get; }

        public Neuron Property { get; }

        public Neuron Class { get; }
    }
}
