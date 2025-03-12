using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyInstanceValueAssociationReader : 
        ExpressionReaderBase<
            IPropertyInstanceValueAssignment,
            IPropertyInstanceValueAssignmentParameterSet,
            IPropertyInstanceValueAssignmentReader,
            IPropertyInstanceValueAssociation,
            IPropertyInstanceValueAssociationParameterSet,
            PropertyInstanceValueAssociation
        >,
        IPropertyInstanceValueAssociationReader
    {
        public PropertyInstanceValueAssociationReader(
            IPropertyInstanceValueAssignmentReader propertyAssignmentReader, 
            IExpressionReader expressionReader, 
            IExternalReferenceSet externalReferences
        ) : base (
            propertyAssignmentReader,
            expressionReader,
            externalReferences
        )
        {
        }

        protected override ExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyInstanceValueAssociationParameterSet parameters,
            Neuron greatGranny
        ) => this.CreateExpressionParameterSet(
            externalReferences,
            (IPropertyParameterSet) parameters,
            greatGranny
        );

        protected override IPropertyInstanceValueAssignmentParameterSet CreateGreatGrannyParameterSet(IPropertyInstanceValueAssociationParameterSet parameters) =>
            new PropertyInstanceValueAssignmentParameterSet(
                parameters.Property,
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );
    }
}
