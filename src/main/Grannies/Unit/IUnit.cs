using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IUnit : IGranny<IUnit, IUnitParameterSet>
    {
        Neuron Value { get; set; }

        Neuron Type { get; set; }
    }
}
