using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IPropertyAssociationParameterSet : IAggregateParameterSet
    {
        Neuron Property { get; }

        Neuron Value { get; }

        Neuron Class { get; }

        ValueMatchByValue ValueMatchBy { get; }
    }
}
