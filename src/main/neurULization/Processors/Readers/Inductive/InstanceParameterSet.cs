using neurUL.Common.Domain.Model;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstanceParameterSet : IInstanceParameterSet
    {
        public InstanceParameterSet(
            Neuron granny,
            Neuron @class,
            IEnumerable<IPropertyAssociationParameterSet> propertyAssociationParameters
            )
        {
            AssertionConcern.AssertArgumentNotNull(granny, nameof(granny));
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));
            AssertionConcern.AssertArgumentNotNull(propertyAssociationParameters, nameof(propertyAssociationParameters));

            this.Granny = granny;
            this.Class = @class;
            this.PropertyAssociationParameters = propertyAssociationParameters;
        }

        public Neuron Granny { get; }

        public Neuron Class { get; }

        public IEnumerable<IPropertyAssociationParameterSet> PropertyAssociationParameters { get; }
    }
}
