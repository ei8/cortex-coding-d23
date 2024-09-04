using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class UnitParameterSet : IUnitParameterSet
    {
        public static UnitParameterSet CreateWithValueAndType(
            Neuron value,
            Neuron type
            )
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));

            return new UnitParameterSet(null, value, type);
        }

        public static UnitParameterSet CreateWithGrannyAndType(
            Neuron granny,
            Neuron type
            )
        {
            AssertionConcern.AssertArgumentNotNull(granny, nameof(granny));

            return new UnitParameterSet(granny, null, type);
        }

        public static UnitParameterSet Create(
            Neuron granny,
            Neuron value,
            Neuron type
            )
        {
            AssertionConcern.AssertArgumentNotNull(granny, nameof(granny));
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));

            return new UnitParameterSet(granny, value, type);
        }

        private UnitParameterSet(
            Neuron granny,
            Neuron value,
            Neuron type
            )
        {
            AssertionConcern.AssertArgumentNotNull(type, nameof(type));

            Granny = granny;
            Value = value;
            Type = type;
        }

        public Neuron Granny { get; }

        public Neuron Value { get; }

        public Neuron Type { get; }
    }
}