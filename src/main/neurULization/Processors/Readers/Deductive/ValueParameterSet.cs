using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ValueParameterSet : IValueParameterSet
    {
        public ValueParameterSet(
            Neuron value,
            Neuron @class,
            ValueMatchBy valueMatchBy
        )
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));

            // TODO: create IInstanceValueParameterSet which accepts @class parameter

            this.Value = value;
            this.Class = @class;
            this.ValueMatchBy = valueMatchBy;
        }

        public Neuron Value { get; }
        public Neuron Class { get; }
        public ValueMatchBy ValueMatchBy { get; }
    }
}
