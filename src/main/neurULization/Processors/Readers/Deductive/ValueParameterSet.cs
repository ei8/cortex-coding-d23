using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ValueParameterSet : IValueParameterSet
    {
        public ValueParameterSet(
            Neuron value
        )
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));

            this.Value = value;
        }

        public Neuron Value { get; }
    }
}
