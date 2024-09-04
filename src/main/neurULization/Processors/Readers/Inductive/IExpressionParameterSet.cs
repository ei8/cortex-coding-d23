using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public interface IExpressionParameterSet : IInductiveParameterSet
    {
        IEnumerable<IUnitParameterSet> UnitParameters { get; }
    }
}
