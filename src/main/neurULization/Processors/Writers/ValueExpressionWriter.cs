using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ValueExpressionWriter : 
        LesserExpressionWriterBase
        <
            IValue,
            IValueParameterSet,
            IValueWriter,
            IValueExpression,
            IValueExpressionParameterSet,
            IValueExpressionReader,
            ValueExpression,
            IExpressionParameterSet,
            IExpressionWriter
        >,
        IValueExpressionWriter
    {
        public ValueExpressionWriter(
            IValueWriter greatGrannyWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IValueExpressionReader reader,
            IMirrorSet mirrors
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            mirrors
        )
        {
        }

        protected override IValueParameterSet CreateGreatGrannyParameterSet(IValueExpressionParameterSet parameters) => 
            new ValueParameterSet(
                parameters.Value
            );

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors, 
            IValueExpressionParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreateValueExpressionParameterSet(mirrors, greatGranny);
    }
}
