using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
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
            IPropertyInstanceValueAssignmentReader greatGrannyReader,
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
            IPropertyInstanceValueAssociationParameterSet parameters,
            IEnumerable<Neuron> grannyCandidates,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueAssociationParameterSet(
            externalReferences,
            parameters,
            grannyCandidates.First()
        );

        protected override IPropertyInstanceValueAssignmentParameterSet CreateGreatGrannyParameterSet(
            IPropertyInstanceValueAssociationParameterSet parameters, 
            Neuron grannyCandidate
        ) => new PropertyInstanceValueAssignmentParameterSet(
            grannyCandidate,
            parameters.Property,
            parameters.Class
        );

        protected override Guid GetValueUnitTypeId(
            IExternalReferenceSet externalReferences
        ) => externalReferences.DirectObject.Id;
    }
}
