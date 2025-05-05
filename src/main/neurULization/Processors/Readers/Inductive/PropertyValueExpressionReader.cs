using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyValueExpressionReader : 
        ExpressionReaderBase<
            IValueExpression,
            IValueExpressionParameterSet,
            IValueExpressionReader,
            IPropertyValueExpression,
            IPropertyValueExpressionParameterSet,
            PropertyValueExpression
        >,
        IPropertyValueExpressionReader
    {
        public PropertyValueExpressionReader(
            IValueExpressionReader greatGrannyReader, 
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
            IPropertyValueExpressionParameterSet parameters, 
            IEnumerable<Neuron> grannyCandidates, 
            Network network
        ) => ProcessorExtensions.CreatePropertyValueExpressionParameterSet(
            externalReferences,
            parameters,
            grannyCandidates.First()
        );

        protected override IValueExpressionParameterSet CreateGreatGrannyParameterSet(
            IPropertyValueExpressionParameterSet parameters, 
            Neuron grannyCandidate
        ) => new ValueExpressionParameterSet(
            grannyCandidate
        );
    }
}
