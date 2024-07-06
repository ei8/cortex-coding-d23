﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyAssignmentParameterSet : PropertyValueExpressionParameterSet, IPropertyAssignmentParameterSet
    {
        public PropertyAssignmentParameterSet(
            Neuron property,
            Neuron value,
            Neuron @class,
            ValueMatchByValue valueMatchBy,
            IEnsembleRepository ensembleRepository,
            string userId
            ) : base(value, @class, valueMatchBy, ensembleRepository, userId)
        {
            this.Property = property;
        }

        public Neuron Property { get; }
    }
}