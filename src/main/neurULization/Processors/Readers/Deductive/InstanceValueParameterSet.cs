using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class InstanceValueParameterSet : IInstanceValueParameterSet
    {
        public InstanceValueParameterSet(
            Neuron value,
            Neuron @class,
            ValueMatchBy valueMatchBy
        )
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));

            this.Value = value;
            this.Class = @class;
            this.ValueMatchBy = valueMatchBy;
        }

        public Neuron Value { get; }
        public Neuron Class { get; }
        public ValueMatchBy ValueMatchBy { get; }
    }
}
