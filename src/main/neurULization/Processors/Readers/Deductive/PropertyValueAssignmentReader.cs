using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyValueAssignmentReader :
        LesserExpressionReaderBase<
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
            IMirrorSet mirrors
        ) : base (propertyValueExpressionReader, expressionReader, mirrors)
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors, 
            IPropertyValueAssignmentParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueAssignmentParameterSet(
            mirrors,
            parameters,
            greatGranny
        );

        protected override IPropertyValueExpressionParameterSet CreateGreatGrannyParameterSet(IPropertyValueAssignmentParameterSet parameters) =>
            new PropertyValueExpressionParameterSet(
                parameters.Value
            );
    }
}
