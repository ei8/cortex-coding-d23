using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IUnitParameterSet : IDeductiveParameterSet
    {
        Neuron Value { get; }

        Neuron Type { get; }
    }
}
