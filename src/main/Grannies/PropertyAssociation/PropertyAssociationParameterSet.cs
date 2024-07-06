using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyAssociationParameterSet : PropertyAssignmentParameterSet, IPropertyAssociationParameterSet
    {
        public PropertyAssociationParameterSet(
            Neuron property,
            Neuron value,
            Neuron @class,
            ValueMatchByValue valueMatchBy,
            IEnsembleRepository ensembleRepository,
            string userId
            ) : base(property, value, @class, valueMatchBy, ensembleRepository, userId)
        {
        }
    }
}
