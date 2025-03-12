using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class InstanceParameterSet : IInstanceParameterSet
    {
        public InstanceParameterSet(
            Guid id,
            string tag,
            string externalReferenceUrl,
            Guid? regionId,
            Neuron @class,
            IEnumerable<IPropertyValueAssociationParameterSet> propertyAssociationsParameters
            )
        {
            AssertionConcern.AssertArgumentValid(i => i != Guid.Empty, id, $"Id cannot be equal to '{Guid.Empty}'.", nameof(id));
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));
            AssertionConcern.AssertArgumentNotNull(propertyAssociationsParameters, nameof(propertyAssociationsParameters));

            Id = id;
            Tag = tag;
            ExternalReferenceUrl = externalReferenceUrl;
            RegionId = regionId;
            Class = @class;
            PropertyValueAssociationsParameters = propertyAssociationsParameters;
        }

        public Guid Id { get; }

        public string Tag { get; }

        public string ExternalReferenceUrl { get; }

        public Guid? RegionId { get; }

        public Neuron Class { get; }

        public IEnumerable<IPropertyValueAssociationParameterSet> PropertyValueAssociationsParameters { get; }
    }
}
