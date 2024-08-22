﻿namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class PropertyAssignmentParameterSet : PropertyValueExpressionParameterSet, IPropertyAssignmentParameterSet
    {
        public PropertyAssignmentParameterSet(
            Neuron property,
            Neuron value,
            Neuron @class,
            ValueMatchBy valueMatchBy
            ) : base(value, @class, valueMatchBy)
        {
            this.Property = property;
        }

        public Neuron Property { get; }
    }
}