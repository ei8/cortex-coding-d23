using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IDependent : IGranny<IDependent, IDependentParameterSet>
    {
        Neuron Value { get; }

        Neuron Type { get; }
    }
}
