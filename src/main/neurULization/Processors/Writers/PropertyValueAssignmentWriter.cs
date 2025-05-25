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
            IMirrorSet mirrors
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            mirrors
        )
        {
        }

        protected override IPropertyValueExpressionParameterSet CreateGreatGrannyParameterSet(IPropertyValueAssignmentParameterSet parameters) =>
            new PropertyValueExpressionParameterSet(
                parameters.Value
            );

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors, 
            IPropertyValueAssignmentParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreatePropertyValueAssignmentParameterSet(mirrors, parameters, greatGranny);        
    }
}
