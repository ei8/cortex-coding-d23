using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class InstanceValueExpressionReader : 
        LesserExpressionReaderBase<
            IInstanceValue,
            IInstanceValueParameterSet,
            IInstanceValueReader,
            IInstanceValueExpression,
            IInstanceValueExpressionParameterSet,
            InstanceValueExpression
        >, 
        IInstanceValueExpressionReader
    {
        public InstanceValueExpressionReader(
            IInstanceValueReader instanceValueReader,
            IExpressionReader expressionReader,
            IMirrorSet mirrors
        ) : base (
            instanceValueReader, 
            expressionReader, 
            mirrors
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors,
            IInstanceValueExpressionParameterSet parameters,
            Neuron greatGranny,
            Network network
        ) => ProcessorExtensions.CreateValueExpressionParameterSet(
            mirrors,
            greatGranny
        );

        protected override IInstanceValueParameterSet CreateGreatGrannyParameterSet(IInstanceValueExpressionParameterSet parameters) =>
            new InstanceValueParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );
    }
}
