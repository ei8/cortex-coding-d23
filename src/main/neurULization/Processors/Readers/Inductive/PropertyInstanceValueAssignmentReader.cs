using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyInstanceValueAssignmentReader :
        ExpressionReaderBase<
            IPropertyInstanceValueExpression,
            IPropertyInstanceValueExpressionParameterSet,
            IPropertyInstanceValueExpressionReader,
            IPropertyInstanceValueAssignment,
            IPropertyInstanceValueAssignmentParameterSet,
            PropertyInstanceValueAssignment
        >,
        IPropertyInstanceValueAssignmentReader
    {
        public PropertyInstanceValueAssignmentReader(
            IPropertyInstanceValueExpressionReader greatGrannyReader, 
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
            IPropertyInstanceValueAssignmentParameterSet parameters,
            IEnumerable<Neuron> grannyCandidates,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueAssignmentParameterSet(
            externalReferences,
            parameters,
            grannyCandidates.First()
        );

        protected override IPropertyInstanceValueExpressionParameterSet CreateGreatGrannyParameterSet(
            IPropertyInstanceValueAssignmentParameterSet parameters, 
            Neuron grannyCandidate
        ) => new PropertyInstanceValueExpressionParameterSet(
            grannyCandidate,
            parameters.Class
        );

        protected override Guid GetValueUnitTypeId(IExternalReferenceSet externalReferences) =>
            externalReferences.NominalModifier.Id;
    }
}
