using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23
{
    public interface IPrimitiveSet
    {
        Neuron DirectObject { get; }

        Neuron Idea { get; }

        Neuron Instantiates { get; }

        Neuron Subordination { get; }

        Neuron Unit { get; }
    }
}
