using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IPropertyValueExpressionParameterSet : IDeductiveParameterSet
    {
        Neuron Value { get; }

        Neuron Class { get; }

        ValueMatchBy ValueMatchBy { get; }
    }
}
