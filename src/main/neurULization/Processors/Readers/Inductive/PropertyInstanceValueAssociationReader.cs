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
            IPropertyInstanceValueAssociationParameterSet parameters,
            IEnumerable<Neuron> grannyCandidates,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueAssociationParameterSet(
            mirrors,
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
            IMirrorSet mirrors
        ) => mirrors.DirectObject.Id;
    }
}
