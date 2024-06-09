using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class HeadParameterSet : IHeadParameterSet
    {
        public HeadParameterSet(Neuron value)
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));

            this.Value = value;
        }

        public Neuron Value { get; }
    }
}
