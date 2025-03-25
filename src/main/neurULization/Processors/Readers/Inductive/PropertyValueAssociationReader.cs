using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
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
            IPropertyValueAssignmentReader greatGrannyReader,
            IExpressionReader expressionReader, 
            IExternalReferenceSet externalReferences,
            IAggregateParser aggregateParser
        ) : base(
            greatGrannyReader,
            expressionReader,
            externalReferences,
            aggregateParser
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyValueAssociationParameterSet parameters,
            Neuron grannyCandidate,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueAssociationParameterSet(
            externalReferences,
            parameters,
            grannyCandidate
        );

        protected override IPropertyValueAssignmentParameterSet CreateGreatGrannyParameterSet(
            IPropertyValueAssociationParameterSet parameters, 
            Neuron grannyCandidate
        ) => new PropertyValueAssignmentParameterSet(
            grannyCandidate,
            parameters.Property
        );

        protected override Guid GetValueUnitTypeId(
            IExternalReferenceSet externalReferences
        ) => externalReferences.DirectObject.Id;
    }
}
