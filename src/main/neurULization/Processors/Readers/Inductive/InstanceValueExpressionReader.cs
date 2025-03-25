using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstanceValueExpressionReader :
        ExpressionReaderBase<
            IInstanceValue,
            IInstanceValueParameterSet,
            IInstanceValueReader,
            IInstanceValueExpression,
            IInstanceValueExpressionParameterSet,
            InstanceValueExpression
        >,
        IInstanceValueExpressionReader
    {
        public InstanceValueExpressionReader(
            IInstanceValueReader greatGrannyReader, 
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
            IInstanceValueExpressionParameterSet parameters,
            Neuron grannyCandidate,
            Network network
        ) => ProcessorExtensions.CreateValueExpressionParameterSet(
            externalReferences,
            parameters,
            grannyCandidate
        );

        protected override IInstanceValueParameterSet CreateGreatGrannyParameterSet(
            IInstanceValueExpressionParameterSet parameters, 
            Neuron grannyCandidate
        ) => new InstanceValueParameterSet(
            grannyCandidate,
            parameters.Class
        );
    }
}
