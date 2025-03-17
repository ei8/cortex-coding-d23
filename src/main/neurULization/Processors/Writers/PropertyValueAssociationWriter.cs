using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyValueAssociationWriter :
        ExpressionWriterBase<
            IPropertyValueAssignment,
            IPropertyValueAssignmentParameterSet,
            IPropertyValueAssignmentWriter,
            IPropertyValueAssociation,
            IPropertyValueAssociationReader,
            IPropertyValueAssociationParameterSet,
            PropertyValueAssociation
        >, 
        IPropertyValueAssociationWriter
    {
        public PropertyValueAssociationWriter(
            IPropertyValueAssignmentWriter greatGrannyWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyValueAssociationReader reader,
            IExternalReferenceSet externalReferences
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            externalReferences
        )
        {
        }

        protected override IPropertyValueAssignmentParameterSet CreateGreatGrannyParameterSet(IPropertyValueAssociationParameterSet parameters) =>
            new PropertyValueAssignmentParameterSet(
                parameters.Property,
                parameters.Value
            );

        protected override ExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IPropertyValueAssociationParameterSet parameters, Neuron greatGranny) =>
            ProcessorExtensions.CreatePropertyValueAssociationParameterSet(externalReferences, parameters, greatGranny);
    }
}
