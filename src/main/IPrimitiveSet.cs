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

        Neuron Simple { get; }

        Neuron Subordination { get; }

        Neuron Coordination { get; }

        Neuron Unit { get; }

        Neuron Of { get; }

        Neuron Case { get; }
    }
}
