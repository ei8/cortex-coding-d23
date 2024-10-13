using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyAssociationParameterSet : IPropertyAssociationParameterSet
    {
        public static PropertyAssociationParameterSet CreateWithoutGranny(
            Neuron property,
            Neuron @class
        ) => new PropertyAssociationParameterSet(null, property, @class);

        public static PropertyAssociationParameterSet CreateWithGranny(
            Neuron granny,
            Neuron property,
            Neuron @class
        )
        {
            AssertionConcern.AssertArgumentNotNull(granny, nameof(granny));

            return new PropertyAssociationParameterSet(granny, property, @class);
        }

        private PropertyAssociationParameterSet(
            Neuron granny,
            Neuron property,
            Neuron @class
            )
        {
            AssertionConcern.AssertArgumentNotNull(property, nameof(property));
            // TODO: create constructor overload that does not require the @class parameter
            // AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));

            this.Granny = granny;
            this.Property = property;
            this.Class = @class;
        }

        public Neuron Granny { get; }

        public Neuron Property { get; }

        public Neuron Class { get; }
    }
}
