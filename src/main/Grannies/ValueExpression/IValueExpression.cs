﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IValueExpression : IGranny<IValueExpression, IValueExpressionParameterSet>
    {
        IInstantiation ValueInstantiation { get; }

        IHead Value { get; }

        IDependent Of { get; }
    }
}
