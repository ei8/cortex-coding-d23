using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
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
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));

            Value = value;
            Class = @class;
            ValueMatchBy = valueMatchBy;
        }

        public Neuron Value { get; }

        public Neuron Class { get; }

        public ValueMatchBy ValueMatchBy { get; }
    }
}
