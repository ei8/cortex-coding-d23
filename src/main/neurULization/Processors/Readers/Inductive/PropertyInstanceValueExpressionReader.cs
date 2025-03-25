using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyInstanceValueExpressionReader : 
        ExpressionReaderBase<
            IInstanceValueExpression,
            IInstanceValueExpressionParameterSet,
            IInstanceValueExpressionReader,
            IPropertyInstanceValueExpression,
            IPropertyInstanceValueExpressionParameterSet,
            PropertyInstanceValueExpression
        >,
        IPropertyInstanceValueExpressionReader
    {
        public PropertyInstanceValueExpressionReader(
            IInstanceValueExpressionReader greatGrannyReader, 
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
            IPropertyInstanceValueExpressionParameterSet parameters, 
            Neuron grannyCandidate, 
            Network network
        ) => ProcessorExtensions.CreatePropertyValueExpressionParameterSet(
            externalReferences,
            parameters,
            grannyCandidate
        );

        protected override IInstanceValueExpressionParameterSet CreateGreatGrannyParameterSet(
            IPropertyInstanceValueExpressionParameterSet parameters, 
            Neuron grannyCandidate
        ) => new InstanceValueExpressionParameterSet(
            grannyCandidate,
            parameters.Class
        );
    }
}
