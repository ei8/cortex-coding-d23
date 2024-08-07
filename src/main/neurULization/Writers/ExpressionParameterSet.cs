using neurUL.Common.Domain.Model;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class ExpressionParameterSet : IExpressionParameterSet
    {
        public ExpressionParameterSet(
            IEnumerable<IUnitParameterSet> unitsParameters
            )
        {
            AssertionConcern.AssertArgumentNotNull(unitsParameters, nameof(unitsParameters));

            UnitsParameters = unitsParameters;
        }

        public IEnumerable<IUnitParameterSet> UnitsParameters { get; }
    }
}
