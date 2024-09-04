using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IExpressionParameterSet : IDeductiveParameterSet
    {
        IEnumerable<IUnitParameterSet> UnitsParameters { get; }
    }
}
