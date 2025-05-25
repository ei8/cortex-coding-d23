using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyInstanceValueAssignmentReader :
        LesserExpressionReaderBase<
            IPropertyInstanceValueExpression,
            IPropertyInstanceValueExpressionParameterSet,
            IPropertyInstanceValueExpressionReader,
            IPropertyInstanceValueAssignment,
            IPropertyInstanceValueAssignmentParameterSet,
            PropertyInstanceValueAssignment
        >,
        IPropertyInstanceValueAssignmentReader
    {
        public PropertyInstanceValueAssignmentReader(
            IPropertyInstanceValueExpressionReader propertyInstanceValueExpressionReader,
            IExpressionReader expressionReader,
            IMirrorSet mirrors
        ) : base(
            propertyInstanceValueExpressionReader,
            expressionReader, 
            mirrors
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors,
            IPropertyInstanceValueAssignmentParameterSet parameters,
            Neuron greatGranny,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueAssignmentParameterSet(
            mirrors,
            parameters,
            greatGranny
        );

        protected override IPropertyInstanceValueExpressionParameterSet CreateGreatGrannyParameterSet(IPropertyInstanceValueAssignmentParameterSet parameters) =>
            new PropertyInstanceValueExpressionParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );
    }
}
