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
            IExternalReferenceSet externalReferences,
            IAggregateParser aggregateParser
        ) : base(
            greatGrannyReader,
            expressionReader,
            externalReferences,
            aggregateParser
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences, 
            IValueExpressionParameterSet parameters, 
            IEnumerable<Neuron> grannyCandidates, 
            Network network
        ) => ProcessorExtensions.CreateValueExpressionParameterSet(
            externalReferences,
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
