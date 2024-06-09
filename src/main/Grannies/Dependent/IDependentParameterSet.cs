using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IDependentParameterSet : IParameterSet
    {
        Neuron Value { get; }

        Neuron Type { get; }
    }
}
