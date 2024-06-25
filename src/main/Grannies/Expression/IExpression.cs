using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IExpression : IGranny<IExpression, IExpressionParameterSet>
    {
        IEnumerable<IUnit> Units { get; }
    }
}