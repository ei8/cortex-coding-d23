using neurUL.Common.Domain.Model;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class ExpressionParameterSet : IExpressionParameterSet
    {
        public ExpressionParameterSet(
            IEnumerable<IUnitParameterSet> dependentsParameters
            )
        {
            AssertionConcern.AssertArgumentNotNull(dependentsParameters, nameof(dependentsParameters));

            this.UnitsParameters = dependentsParameters;
        }

        public IEnumerable<IUnitParameterSet> UnitsParameters { get; }
    }
}
