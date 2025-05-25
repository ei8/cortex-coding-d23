using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyValueExpressionReader :
        LesserExpressionReaderBase<
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
            IMirrorSet mirrors
        ) : base ( 
            valueExpressionReader,
            expressionReader,
            mirrors
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors, 
            IPropertyValueExpressionParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueExpressionParameterSet(
            mirrors,
            greatGranny
        );

        protected override IValueExpressionParameterSet CreateGreatGrannyParameterSet(IPropertyValueExpressionParameterSet parameters) =>
            new ValueExpressionParameterSet(
                parameters.Value
            );
    }
}
