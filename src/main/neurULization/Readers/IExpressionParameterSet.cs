using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    public interface IExpressionParameterSet : IReadParameterSet
    {
        IEnumerable<IUnitParameterSet> UnitParameters { get; }
    }
}
