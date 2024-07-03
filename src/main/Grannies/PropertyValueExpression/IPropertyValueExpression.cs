﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IPropertyValueExpression : IGranny<IPropertyValueExpression, IPropertyValueExpressionParameterSet>
    {
        IValueExpression ValueExpression { get; }
        
        IExpression Expression { get; }
    }
}
