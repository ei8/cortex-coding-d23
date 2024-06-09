using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IHead : IGranny<IHead, IHeadParameterSet>
    {
        Neuron Value { get; }
    }
}
