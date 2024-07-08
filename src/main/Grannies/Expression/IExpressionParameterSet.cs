using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IExpressionParameterSet : IParameterSet
    {
        IEnumerable<IUnitParameterSet> UnitsParameters { get; }
    }
}
