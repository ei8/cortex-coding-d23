using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyValueAssociationReader : 
        ExpressionReaderBase<
            IPropertyValueAssignment,
            IPropertyValueAssignmentParameterSet,
            IPropertyValueAssignmentReader,
            IPropertyValueAssociation,
            IPropertyValueAssociationParameterSet,
            PropertyValueAssociation
        >,
        IPropertyValueAssociationReader
    {
        public PropertyValueAssociationReader(
            IPropertyValueAssignmentReader propertyAssignmentReader, 
            IExpressionReader expressionReader, 
            IExternalReferenceSet externalReferences
        ) : base (
            propertyAssignmentReader,
            expressionReader,
            externalReferences
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyValueAssociationParameterSet parameters,
            Neuron greatGranny,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueAssociationParameterSet(
            externalReferences,
            greatGranny
        );

        protected override IPropertyValueAssignmentParameterSet CreateGreatGrannyParameterSet(IPropertyValueAssociationParameterSet parameters) =>
            new PropertyValueAssignmentParameterSet(
                parameters.Property,
                parameters.Value
            );
    }
}
