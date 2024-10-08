﻿namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyAssociationParameterSet : PropertyAssignmentParameterSet, IPropertyAssociationParameterSet
    {
        public PropertyAssociationParameterSet(
            Neuron property,
            Neuron value,
            Neuron @class,
            ValueMatchBy valueMatchBy
            ) : base(property, value, @class, valueMatchBy)
        {
        }
    }
}
