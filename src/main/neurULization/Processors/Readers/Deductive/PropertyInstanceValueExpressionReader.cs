using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyInstanceValueExpressionReader :
        LesserExpressionReaderBase<
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
            IInstanceValueExpressionReader instanceValueExpressionReader,
            IExpressionReader expressionReader,
            IMirrorSet mirrors
        ) : base(
            instanceValueExpressionReader,
            expressionReader,
            mirrors
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors, 
            IPropertyInstanceValueExpressionParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueExpressionParameterSet(
            mirrors,
            greatGranny
        );

        protected override IInstanceValueExpressionParameterSet CreateGreatGrannyParameterSet(IPropertyInstanceValueExpressionParameterSet parameters) =>
            new InstanceValueExpressionParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );
    }
}
