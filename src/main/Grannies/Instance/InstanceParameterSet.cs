using neurUL.Common.Domain.Model;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstanceParameterSet : IInstanceParameterSet
    {
        public InstanceParameterSet(
            Neuron @class,
            IEnumerable<IPropertyAssociationParameterSet> propertyAssociationsParameters
            )
        {
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));
            AssertionConcern.AssertArgumentNotNull(propertyAssociationsParameters, nameof(propertyAssociationsParameters));

            this.Class = @class;
            this.PropertyAssociationsParameters = propertyAssociationsParameters;
        }
        
        public Neuron Class { get; }

        public IEnumerable<IPropertyAssociationParameterSet> PropertyAssociationsParameters { get; }
    }
}
