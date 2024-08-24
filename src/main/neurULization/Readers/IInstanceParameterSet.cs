using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    public interface IInstanceParameterSet : IReadParameterSet
    {
        Neuron Class { get; }

        IEnumerable<IPropertyAssociationParameterSet> PropertyAssociationsParameters { get; }
    }
}
