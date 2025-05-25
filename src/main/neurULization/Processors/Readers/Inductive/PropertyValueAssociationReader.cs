using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Linq;

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
            IMirrorSet mirrors,
            IAggregateParser aggregateParser
        ) : base(
            greatGrannyReader,
            expressionReader,
            mirrors,
            aggregateParser
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors,
            IPropertyValueAssociationParameterSet parameters,
            IEnumerable<Neuron> grannyCandidates,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueAssociationParameterSet(
            mirrors,
            parameters,
            grannyCandidates.First()
        );

        protected override IPropertyValueAssignmentParameterSet CreateGreatGrannyParameterSet(
            IPropertyValueAssociationParameterSet parameters, 
            Neuron grannyCandidate
        ) => new PropertyValueAssignmentParameterSet(
            grannyCandidate,
            parameters.Property
        );

        protected override Guid GetValueUnitTypeId(
            IMirrorSet mirrors
        ) => mirrors.DirectObject.Id;
    }
}
