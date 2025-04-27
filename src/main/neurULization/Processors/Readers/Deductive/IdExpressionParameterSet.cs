using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class IdExpressionParameterSet : ExpressionParameterSet, IIdExpressionParameterSet
    {
        public IdExpressionParameterSet(
            IEnumerable<IUnitParameterSet> unitParameters,
            Guid id
            ) : base(unitParameters)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}
