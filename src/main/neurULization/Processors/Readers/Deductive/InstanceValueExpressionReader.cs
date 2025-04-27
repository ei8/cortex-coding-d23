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
            IExternalReferenceSet externalReferences
        ) : base (
            instanceValueReader, 
            expressionReader, 
            externalReferences
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IInstanceValueExpressionParameterSet parameters,
            Neuron greatGranny,
            Network network
        ) => ProcessorExtensions.CreateValueExpressionParameterSet(
            externalReferences,
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
