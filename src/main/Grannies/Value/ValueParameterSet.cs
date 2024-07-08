using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class ValueParameterSet : IValueParameterSet
    {
        public ValueParameterSet(
            Neuron value,
            Neuron @class,
            ValueMatchByValue valueMatchBy
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

        public ValueMatchByValue ValueMatchBy { get; }
    }
}
