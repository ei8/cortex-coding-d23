using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IInstanceParameterSet : IDeductiveParameterSet
    {
        Guid Id { get; }

        string Tag { get; }

        string MirrorUrl { get; }

        Guid? RegionId { get; }

        Neuron Class { get; }

        IEnumerable<IPropertyAssociationParameterSet> PropertyAssociationsParameters { get; }
    }
}
