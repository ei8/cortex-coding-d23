using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyValueAssociationWriter :
        LesserExpressionWriterBase
        <
            IPropertyValueAssignment,
            IPropertyValueAssignmentParameterSet,
            IPropertyValueAssignmentWriter,
            IPropertyValueAssociation,
            IPropertyValueAssociationParameterSet,
            IPropertyValueAssociationReader,
            PropertyValueAssociation,
            IExpressionParameterSet,
            IExpressionWriter
        >, 
        IPropertyValueAssociationWriter
    {
        public PropertyValueAssociationWriter(
            IPropertyValueAssignmentWriter greatGrannyWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyValueAssociationReader reader,
            IMirrorSet mirrors
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            mirrors
        )
        {
        }

        protected override IPropertyValueAssignmentParameterSet CreateGreatGrannyParameterSet(IPropertyValueAssociationParameterSet parameters) =>
            new PropertyValueAssignmentParameterSet(
                parameters.Property,
                parameters.Value
            );

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors, 
            IPropertyValueAssociationParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreatePropertyValueAssociationParameterSet(mirrors, greatGranny);
    }
}
