using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ValueExpressionWriter : 
        ExpressionWriterBase<
            IValue,
            IValueParameterSet,
            IValueWriter,
            IValueExpression,
            IValueExpressionReader,
            IValueExpressionParameterSet,
            ValueExpression
        >,
        IValueExpressionWriter
    {
        public ValueExpressionWriter(
            IValueWriter greatGrannyWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IValueExpressionReader reader,
            IExternalReferenceSet externalReferences
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            externalReferences
        )
        {
        }

        protected override IValueParameterSet CreateGreatGrannyParameterSet(IValueExpressionParameterSet parameters) => 
            new ValueParameterSet(
                parameters.Value
            );

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences, 
            IValueExpressionParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreateValueExpressionParameterSet(externalReferences, greatGranny);
    }
}
