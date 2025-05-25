using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class InstanceValueExpressionWriter : 
        LesserExpressionWriterBase
        <
            IInstanceValue,
            IInstanceValueParameterSet,
            IInstanceValueWriter,
            IInstanceValueExpression,
            IInstanceValueExpressionParameterSet,
            IInstanceValueExpressionReader,
            InstanceValueExpression,
            IExpressionParameterSet,
            IExpressionWriter
        >,
        IInstanceValueExpressionWriter
    {
        public InstanceValueExpressionWriter(
            IInstanceValueWriter greatGrannyWriter,
            IExpressionWriter expressionWriter,
            Readers.Deductive.IInstanceValueExpressionReader reader,
            IMirrorSet mirrors
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            mirrors
        )
        {
        }

        protected override IInstanceValueParameterSet CreateGreatGrannyParameterSet(IInstanceValueExpressionParameterSet parameters) =>
            new InstanceValueParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors, 
            IInstanceValueExpressionParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreateValueExpressionParameterSet(mirrors, greatGranny);
    }
}
