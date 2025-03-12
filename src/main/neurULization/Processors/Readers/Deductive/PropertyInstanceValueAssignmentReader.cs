using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyInstanceValueAssignmentReader :
        ExpressionReaderBase<
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
            IExternalReferenceSet externalReferences
        ) : base(
            propertyInstanceValueExpressionReader,
            expressionReader, 
            externalReferences
        )
        {
        }

        protected override ExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyInstanceValueAssignmentParameterSet parameters,
            Neuron greatGranny
        ) => this.CreateExpressionParameterSet(
            externalReferences,
            (IPropertyParameterSet) parameters,
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
