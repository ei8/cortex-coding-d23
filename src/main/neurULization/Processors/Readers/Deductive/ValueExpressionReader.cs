using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
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
            IValueReader valueReader, 
            IExpressionReader expressionReader, 
            IExternalReferenceSet externalReferences
        ) : base (
            valueReader, 
            expressionReader, 
            externalReferences
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences, 
            IValueExpressionParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) => ProcessorExtensions.CreateValueExpressionParameterSet(
            externalReferences,
            greatGranny
        );

        protected override IValueParameterSet CreateGreatGrannyParameterSet(IValueExpressionParameterSet parameters) =>
            new ValueParameterSet(
                parameters.Value
            );        
    }
}
