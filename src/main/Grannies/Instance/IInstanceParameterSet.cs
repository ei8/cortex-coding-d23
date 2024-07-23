using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IInstanceParameterSet : IParameterSet
    {
        Guid Id { get; }

        string Tag { get; }

        string ExternalReferenceUrl { get; }

        Guid? RegionId { get; }

        Neuron Class { get; }

        IEnumerable<IPropertyAssociationParameterSet> PropertyAssociationsParameters { get; }
    }
}
