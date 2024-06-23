using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
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

            this.Value = value;
            this.Type = type;
        }

        public Neuron Value { get; }

        public Neuron Type { get; }
    }
}