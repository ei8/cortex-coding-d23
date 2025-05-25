using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyValueAssociationReader : 
        LesserExpressionReaderBase<
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
            IPropertyValueAssociationParameterSet parameters,
            Neuron greatGranny,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueAssociationParameterSet(
            mirrors,
            greatGranny
        );

        protected override IPropertyValueAssignmentParameterSet CreateGreatGrannyParameterSet(IPropertyValueAssociationParameterSet parameters) =>
            new PropertyValueAssignmentParameterSet(
                parameters.Property,
                parameters.Value
            );
    }
}
