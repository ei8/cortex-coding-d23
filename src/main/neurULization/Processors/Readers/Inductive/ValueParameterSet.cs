using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ValueParameterSet : IValueParameterSet
    {
        public ValueParameterSet(
            Neuron granny
        )
        {
            AssertionConcern.AssertArgumentNotNull(granny, nameof(granny));
            
            this.Granny = granny;
        }

        public Neuron Granny { get; }
    }
}
