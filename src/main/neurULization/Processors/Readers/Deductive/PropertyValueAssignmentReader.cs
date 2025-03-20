using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyValueAssignmentReader :
        ExpressionReaderBase<
            IPropertyValueExpression,
            IPropertyValueExpressionParameterSet,
            IPropertyValueExpressionReader,
            IPropertyValueAssignment,
            IPropertyValueAssignmentParameterSet,
            PropertyValueAssignment
        >,
        IPropertyValueAssignmentReader
    {
        public PropertyValueAssignmentReader(
            IPropertyValueExpressionReader propertyValueExpressionReader, 
            IExpressionReader expressionReader, 
            IExternalReferenceSet externalReferences
        ) : base (propertyValueExpressionReader, expressionReader, externalReferences)
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences, 
            IPropertyValueAssignmentParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueAssignmentParameterSet(
            externalReferences,
            parameters,
            greatGranny
        );

        protected override IPropertyValueExpressionParameterSet CreateGreatGrannyParameterSet(IPropertyValueAssignmentParameterSet parameters) =>
            new PropertyValueExpressionParameterSet(
                parameters.Value
            );
    }
}
