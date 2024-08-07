using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public interface IUnitParameterSet : IWriteParameterSet
    {
        Neuron Value { get; }

        Neuron Type { get; }
    }
}
