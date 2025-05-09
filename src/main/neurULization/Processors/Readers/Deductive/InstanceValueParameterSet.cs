using neurUL.Common.Domain.Model;
using System;

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
            AssertionConcern.AssertArgumentValid(
                p => valueMatchBy != ValueMatchBy.Tag || p.Tag != null, 
                value, 
                $"Tag of specified '{nameof(value)}' neuron cannot be null when '{nameof(valueMatchBy)}' is equal to '{valueMatchBy.ToString()}'.", 
                nameof(value)
            );
            AssertionConcern.AssertArgumentValid(
                p => valueMatchBy != ValueMatchBy.Id || p.Id != Guid.Empty,
                value,
                $"Id of specified '{nameof(value)}' neuron cannot be equal to '{Guid.Empty}' when '{nameof(valueMatchBy)}' is equal to '{valueMatchBy.ToString()}'.",
                nameof(value)
            );
            AssertionConcern.AssertArgumentValid(
                vmb => valueMatchBy != ValueMatchBy.NotSet,
                valueMatchBy,
                $"Specified '{nameof(valueMatchBy)}' value of '{nameof(ValueMatchBy.NotSet)}' is not valid.",
                nameof(valueMatchBy)
            );

            this.Value = value;
            this.Class = @class;
            this.ValueMatchBy = valueMatchBy;
        }

        public Neuron Value { get; }
        public Neuron Class { get; }
        public ValueMatchBy ValueMatchBy { get; }
    }
}
