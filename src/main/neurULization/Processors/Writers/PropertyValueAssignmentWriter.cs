using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyValueAssignmentWriter :
        ExpressionWriterBase<
            IPropertyValueExpression,
            IPropertyValueExpressionParameterSet,
            IPropertyValueExpressionWriter,
            IPropertyValueAssignment,
            IPropertyValueAssignmentReader,
            IPropertyValueAssignmentParameterSet,
            PropertyValueAssignment
        >,
        IPropertyValueAssignmentWriter
    {
        public PropertyValueAssignmentWriter(
            IPropertyValueExpressionWriter greatGrannyWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyValueAssignmentReader reader,
            IExternalReferenceSet externalReferences
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            externalReferences
        )
        {
        }

        protected override IPropertyValueExpressionParameterSet CreateGreatGrannyParameterSet(IPropertyValueAssignmentParameterSet parameters) =>
            new PropertyValueExpressionParameterSet(
                parameters.Value
            );

        protected override ExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IPropertyValueAssignmentParameterSet parameters, Neuron greatGranny) =>
            ProcessorExtensions.CreatePropertyValueAssignmentParameterSet(externalReferences, parameters, greatGranny);        
    }
}
