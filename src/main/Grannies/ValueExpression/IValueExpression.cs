using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IValueExpression : IGranny<IValueExpression, IValueExpressionParameterSet>
    {
        IValue ValueInstantiation { get; }

        IUnit Value { get; }

        IUnit Of { get; }
    }
}
