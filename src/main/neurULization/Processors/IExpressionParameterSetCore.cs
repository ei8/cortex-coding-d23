using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors
{
    public interface IExpressionParameterSetCore<TUnitParameterSet> : IParameterSet
        where TUnitParameterSet : IUnitParameterSetCore
    {
        IEnumerable<TUnitParameterSet> UnitParameters { get; }
    }
}
