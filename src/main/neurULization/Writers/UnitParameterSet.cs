using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class UnitParameterSet : IUnitParameterSet
    {
        public UnitParameterSet(
            Neuron value,
            Neuron type
            )
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            AssertionConcern.AssertArgumentNotNull(type, nameof(type));

            Value = value;
            Type = type;
        }

        public Neuron Value { get; }

        public Neuron Type { get; }
    }
}