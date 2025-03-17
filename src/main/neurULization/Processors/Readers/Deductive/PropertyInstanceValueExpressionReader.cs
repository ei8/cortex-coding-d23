using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
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
            IInstanceValueExpressionReader instanceValueExpressionReader,
            IExpressionReader expressionReader,
            IExternalReferenceSet externalReferences
        ) : base(
            instanceValueExpressionReader,
            expressionReader,
            externalReferences
        )
        {
        }

        protected override ExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences, 
            IPropertyInstanceValueExpressionParameterSet parameters, 
            Neuron greatGranny
        ) => ProcessorExtensions.CreatePropertyValueExpressionParameterSet(
            externalReferences,
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
