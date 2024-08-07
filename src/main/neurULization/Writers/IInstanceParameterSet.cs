using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public interface IInstanceParameterSet : IWriteParameterSet
    {
        Guid Id { get; }

        string Tag { get; }

        string ExternalReferenceUrl { get; }

        Guid? RegionId { get; }

        Neuron Class { get; }

        IEnumerable<IPropertyAssociationParameterSet> PropertyAssociationsParameters { get; }
    }
}
