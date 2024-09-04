using neurUL.Common.Domain.Model;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ExpressionParameterSet : IExpressionParameterSet
    {
        public ExpressionParameterSet(
            Neuron granny,
            IEnumerable<IUnitParameterSet> unitParameters
            )
        {
            AssertionConcern.AssertArgumentNotNull(granny, nameof(granny));
            AssertionConcern.AssertArgumentNotNull(unitParameters, nameof(unitParameters));

            Granny = granny;
            UnitParameters = unitParameters;
        }

        public Neuron Granny { get; }

        public IEnumerable<IUnitParameterSet> UnitParameters { get; }
    }
}
