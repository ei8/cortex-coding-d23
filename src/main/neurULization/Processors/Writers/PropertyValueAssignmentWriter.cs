using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyValueAssignmentWriter :
        LesserExpressionWriterBase
        <
            IPropertyValueExpression,
            IPropertyValueExpressionParameterSet,
            IPropertyValueExpressionWriter,
            IPropertyValueAssignment,
            IPropertyValueAssignmentParameterSet,
            IPropertyValueAssignmentReader,
            PropertyValueAssignment,
            IExpressionParameterSet,
            IExpressionWriter
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

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences, 
            IPropertyValueAssignmentParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreatePropertyValueAssignmentParameterSet(externalReferences, parameters, greatGranny);        
    }
}
