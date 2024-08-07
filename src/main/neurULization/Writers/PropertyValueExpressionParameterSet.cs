using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class PropertyValueExpressionParameterSet : ValueExpressionParameterSet, IPropertyValueExpressionParameterSet
    {
        public PropertyValueExpressionParameterSet(
            Neuron value,
            Neuron @class,
            ValueMatchBy valueMatchBy
            ) : base(value, @class, valueMatchBy)
        {
        }
    }
}
