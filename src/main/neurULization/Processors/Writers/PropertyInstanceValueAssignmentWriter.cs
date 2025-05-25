using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyInstanceValueAssignmentWriter :
        LesserExpressionWriterBase
        <
            IPropertyInstanceValueExpression,
            IPropertyInstanceValueExpressionParameterSet,
            IPropertyInstanceValueExpressionWriter,
            IPropertyInstanceValueAssignment,
            IPropertyInstanceValueAssignmentParameterSet,
            IPropertyInstanceValueAssignmentReader,
            PropertyInstanceValueAssignment,
            IExpressionParameterSet,
            IExpressionWriter
        >,
        IPropertyInstanceValueAssignmentWriter
    {
        public PropertyInstanceValueAssignmentWriter(
            IPropertyInstanceValueExpressionWriter greatGrannyWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyInstanceValueAssignmentReader reader,
            IMirrorSet mirrors
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            mirrors
        )
        {
        }

        protected override IPropertyInstanceValueExpressionParameterSet CreateGreatGrannyParameterSet(IPropertyInstanceValueAssignmentParameterSet parameters) =>
            new PropertyInstanceValueExpressionParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors, 
            IPropertyInstanceValueAssignmentParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreatePropertyValueAssignmentParameterSet(mirrors, parameters, greatGranny);        
    }
}
