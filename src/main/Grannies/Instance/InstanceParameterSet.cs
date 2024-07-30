using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstanceParameterSet : IInstanceParameterSet
    {
        public InstanceParameterSet(
            Guid id,
            string tag,
            string externalReferenceUrl,
            Guid? regionId,
            Neuron @class,
            IEnumerable<IPropertyAssociationParameterSet> propertyAssociationsParameters,
            WriteMode writeMode
            )
        {
            AssertionConcern.AssertArgumentValid(i => i != Guid.Empty, id, $"Id cannot be equal to '{Guid.Empty}'.", nameof(id));
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));
            AssertionConcern.AssertArgumentNotNull(propertyAssociationsParameters, nameof(propertyAssociationsParameters));

            this.Id = id;
            this.Tag = tag;
            this.ExternalReferenceUrl = externalReferenceUrl;
            this.RegionId = regionId;
            this.Class = @class;
            this.PropertyAssociationsParameters = propertyAssociationsParameters;
            this.WriteMode = writeMode;
        }

        public Guid Id { get; }

        public string Tag { get; }

        public string ExternalReferenceUrl { get; }

        public Guid? RegionId { get; }

        public Neuron Class { get; }

        public IEnumerable<IPropertyAssociationParameterSet> PropertyAssociationsParameters { get; }

        public WriteMode WriteMode { get; }
    }
}
