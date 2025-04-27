using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ExpressionParameterSet : IExpressionParameterSet
    {
        public ExpressionParameterSet(
            Neuron granny,
            IEnumerable<IUnitParameterSet> unitParameters,
            // TODO:0 remove optional parameter?
            Guid? id = null
            )
        {
            AssertionConcern.AssertArgumentNotNull(granny, nameof(granny));
            AssertionConcern.AssertArgumentNotNull(unitParameters, nameof(unitParameters));

            this.Granny = granny;
            this.UnitParameters = unitParameters;
            this.Id = id;
        }

        public Neuron Granny { get; }

        public IEnumerable<IUnitParameterSet> UnitParameters { get; }

        public Guid? Id { get; }
    }
}
