using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
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
            IValueExpressionReader valueExpressionReader, 
            IExpressionReader expressionReader, 
            IExternalReferenceSet externalReferences
        ) : base ( 
            valueExpressionReader,
            expressionReader,
            externalReferences
        )
        {
        }

        protected override ExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences, 
            IPropertyValueExpressionParameterSet parameters, 
            Neuron greatGranny
        ) => ProcessorExtensions.CreatePropertyValueExpressionParameterSet(
            externalReferences,
            greatGranny
        );

        protected override IValueExpressionParameterSet CreateGreatGrannyParameterSet(IPropertyValueExpressionParameterSet parameters) =>
            new ValueExpressionParameterSet(
                parameters.Value
            );
    }
}
