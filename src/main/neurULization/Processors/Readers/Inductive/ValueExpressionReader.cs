using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ValueExpressionReader :
        ExpressionReaderBase<
            IValue,
            IValueParameterSet,
            IValueReader,
            IValueExpression,
            IValueExpressionParameterSet,
            ValueExpression
        >,
        IValueExpressionReader
    {
        public ValueExpressionReader(
            IValueReader greatGrannyReader,
            IExpressionReader expressionReader,
            IMirrorSet mirrors,
            IAggregateParser aggregateParser
        ) : base(
            greatGrannyReader,
            expressionReader,
            mirrors,
            aggregateParser
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors, 
            IValueExpressionParameterSet parameters, 
            IEnumerable<Neuron> grannyCandidates, 
            Network network
        ) => ProcessorExtensions.CreateValueExpressionParameterSet(
            mirrors,
            parameters,
            grannyCandidates.First()
        );

        protected override IValueParameterSet CreateGreatGrannyParameterSet(
            IValueExpressionParameterSet parameters, 
            Neuron grannyCandidate
        ) => new ValueParameterSet(
            grannyCandidate
        );
    }
}
