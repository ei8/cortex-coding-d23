using neurUL.Common.Domain.Model;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ExpressionParameterSet : IExpressionParameterSet
    {
        public ExpressionParameterSet(
            IEnumerable<IUnitParameterSet> unitParameters
            )
        {
            AssertionConcern.AssertArgumentNotNull(unitParameters, nameof(unitParameters));

            this.UnitParameters = unitParameters;
        }

        public IEnumerable<IUnitParameterSet> UnitParameters { get; }
    }
}
