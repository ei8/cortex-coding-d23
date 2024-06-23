using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IExpression : IGranny<IExpression, IExpressionParameterSet>
    {
        IUnit Head { get; }

        IEnumerable<IUnit> Dependents { get; }
    }
}