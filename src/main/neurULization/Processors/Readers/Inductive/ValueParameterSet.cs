using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ValueParameterSet : IValueParameterSet
    {
        public ValueParameterSet(
            Neuron granny,
            Neuron @class
            )
        {
            AssertionConcern.AssertArgumentNotNull(granny, nameof(granny));
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));

            Granny = granny;
            Class = @class;
        }

        public Neuron Granny { get; }

        public Neuron Class { get; }
    }
}
