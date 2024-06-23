using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IValueParameterSet : IAggregateParameterSet
    {
        Neuron Value { get; }

        Neuron Class { get; }

        InstantiationMatchingNeuronProperty MatchingNeuronProperty { get; }
    }
}
