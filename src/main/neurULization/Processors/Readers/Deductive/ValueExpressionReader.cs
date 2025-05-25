using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ValueExpressionReader :
        LesserExpressionReaderBase<
            IValue,
            IValueParameterSet,
            IValueReader,
            IValueExpression,
            IValueExpressionParameterSet,
            ValueExpression
        >,
        IValueExpressionReader
    {
        public ValueExpressionReader(
            IValueReader valueReader, 
            IExpressionReader expressionReader, 
            IMirrorSet mirrors
        ) : base (
            valueReader, 
            expressionReader, 
            mirrors
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors, 
            IValueExpressionParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) => ProcessorExtensions.CreateValueExpressionParameterSet(
            mirrors,
            greatGranny
        );

        protected override IValueParameterSet CreateGreatGrannyParameterSet(IValueExpressionParameterSet parameters) =>
            new ValueParameterSet(
                parameters.Value
            );        
    }
}
