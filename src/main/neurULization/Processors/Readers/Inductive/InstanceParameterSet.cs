using neurUL.Common.Domain.Model;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstanceParameterSet : IInstanceParameterSet
    {
        public InstanceParameterSet(
            Neuron granny,
            Neuron @class,
            IEnumerable<IPropertyAssociationParameterSet> propertyAssociationsParameters
            )
        {
            AssertionConcern.AssertArgumentNotNull(granny, nameof(granny));
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));
            AssertionConcern.AssertArgumentNotNull(propertyAssociationsParameters, nameof(propertyAssociationsParameters));

            Granny = granny;
            Class = @class;
            PropertyAssociationsParameters = propertyAssociationsParameters;
        }

        public Neuron Granny { get; }

        public Neuron Class { get; }

        public IEnumerable<IPropertyAssociationParameterSet> PropertyAssociationsParameters { get; }
    }
}
