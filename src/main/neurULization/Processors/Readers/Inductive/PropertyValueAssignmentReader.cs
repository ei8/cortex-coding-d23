using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyValueAssignmentReader :
        ExpressionReaderBase<
            IPropertyValueExpression,
            IPropertyValueExpressionParameterSet,
            IPropertyValueExpressionReader,
            IPropertyValueAssignment,
            IPropertyValueAssignmentParameterSet,
            PropertyValueAssignment
        >,
        IPropertyValueAssignmentReader
    {
        public PropertyValueAssignmentReader(
            IPropertyValueExpressionReader greatGrannyReader, 
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
            IPropertyValueAssignmentParameterSet parameters,
            IEnumerable<Neuron> grannyCandidates,
            Network network
        ) => ProcessorExtensions.CreatePropertyValueAssignmentParameterSet(
            mirrors,
            parameters,
            grannyCandidates.First()
        );

        protected override IPropertyValueExpressionParameterSet CreateGreatGrannyParameterSet(
            IPropertyValueAssignmentParameterSet parameters, 
            Neuron grannyCandidate
        ) => new PropertyValueExpressionParameterSet(
            grannyCandidate
        );

        protected override Guid GetValueUnitTypeId(IMirrorSet mirrors) =>
            mirrors.NominalModifier.Id;
    }
}
