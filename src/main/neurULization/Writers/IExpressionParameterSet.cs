using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public interface IExpressionParameterSet : IWriteParameterSet
    {
        IEnumerable<IUnitParameterSet> UnitsParameters { get; }
    }
}
