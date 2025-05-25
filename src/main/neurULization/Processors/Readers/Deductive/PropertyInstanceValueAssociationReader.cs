using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyInstanceValueAssociationReader : 
        LesserExpressionReaderBase<
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
            IMirrorSet mirrors
        ) : base (
            propertyAssignmentReader,
            expressionReader,
            mirrors
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors,
            IPropertyInstanceValueAssociationParameterSet parameters,
            Neuron greatGranny,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueAssociationParameterSet(
            mirrors,
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
