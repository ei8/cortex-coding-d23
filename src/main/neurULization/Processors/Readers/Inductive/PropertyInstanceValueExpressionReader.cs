using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyInstanceValueExpressionReader : 
        ExpressionReaderBase<
            IInstanceValueExpression,
            IInstanceValueExpressionParameterSet,
            IInstanceValueExpressionReader,
            IPropertyInstanceValueExpression,
            IPropertyInstanceValueExpressionParameterSet,
            PropertyInstanceValueExpression
        >,
        IPropertyInstanceValueExpressionReader
    {
        public PropertyInstanceValueExpressionReader(
            IInstanceValueExpressionReader greatGrannyReader, 
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
            IPropertyInstanceValueExpressionParameterSet parameters, 
            IEnumerable<Neuron> grannyCandidates, 
            Network network
        ) => ProcessorExtensions.CreatePropertyValueExpressionParameterSet(
            mirrors,
            parameters,
            grannyCandidates.First()
        );

        protected override IInstanceValueExpressionParameterSet CreateGreatGrannyParameterSet(
            IPropertyInstanceValueExpressionParameterSet parameters, 
            Neuron grannyCandidate
        ) => new InstanceValueExpressionParameterSet(
            grannyCandidate,
            parameters.Class
        );
    }
}
