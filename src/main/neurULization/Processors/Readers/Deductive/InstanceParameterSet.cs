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
            string mirrorUrl,
            Guid? regionId,
            Neuron @class,
            IEnumerable<IPropertyAssociationParameterSet> propertyAssociationsParameters
            )
        {
            AssertionConcern.AssertArgumentValid(i => i != Guid.Empty, id, $"Id cannot be equal to '{Guid.Empty}'.", nameof(id));
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));
            AssertionConcern.AssertArgumentNotNull(propertyAssociationsParameters, nameof(propertyAssociationsParameters));

            Id = id;
            Tag = tag;
            MirrorUrl = mirrorUrl;
            RegionId = regionId;
            Class = @class;
            PropertyAssociationsParameters = propertyAssociationsParameters;
        }

        public Guid Id { get; }

        public string Tag { get; }

        public string MirrorUrl { get; }

        public Guid? RegionId { get; }

        public Neuron Class { get; }

        public IEnumerable<IPropertyAssociationParameterSet> PropertyAssociationsParameters { get; }
    }
}
