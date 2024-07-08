﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyValueExpressionParameterSet : ValueExpressionParameterSet, IPropertyValueExpressionParameterSet
    {
        public PropertyValueExpressionParameterSet(
            Neuron value,
            Neuron @class,
            ValueMatchByValue valueMatchBy
            ) : base(value, @class, valueMatchBy)
        {
        }
    }
}
