using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyInstanceValueAssociationWriter :
        LesserExpressionWriterBase
        <
            IPropertyInstanceValueAssignment,
            IPropertyInstanceValueAssignmentParameterSet,
            IPropertyInstanceValueAssignmentWriter,
            IPropertyInstanceValueAssociation,
            IPropertyInstanceValueAssociationParameterSet,
            IPropertyInstanceValueAssociationReader,
            PropertyInstanceValueAssociation,
            IExpressionParameterSet,
            IExpressionWriter
        >, 
        IPropertyInstanceValueAssociationWriter
    {
        public PropertyInstanceValueAssociationWriter(
            IPropertyInstanceValueAssignmentWriter greatGrannyWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyInstanceValueAssociationReader reader,
            IExternalReferenceSet externalReferences
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            externalReferences
        )
        {
        }

        protected override IPropertyInstanceValueAssignmentParameterSet CreateGreatGrannyParameterSet(IPropertyInstanceValueAssociationParameterSet parameters) =>
            new PropertyInstanceValueAssignmentParameterSet(
                parameters.Property,
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences, 
            IPropertyInstanceValueAssociationParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreatePropertyValueAssociationParameterSet(externalReferences, greatGranny);
    }
}
