using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class InstanceValueExpressionWriter : 
        ExpressionWriterBase<
            IInstanceValue,
            IInstanceValueParameterSet,
            IInstanceValueWriter,
            IInstanceValueExpression,
            IInstanceValueExpressionReader,
            IInstanceValueExpressionParameterSet,
            InstanceValueExpression
        >,
        IInstanceValueExpressionWriter
    {
        public InstanceValueExpressionWriter(
            IInstanceValueWriter greatGrannyWriter,
            IExpressionWriter expressionWriter,
            Readers.Deductive.IInstanceValueExpressionReader reader,
            IExternalReferenceSet externalReferences
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            externalReferences
        )
        {
        }

        protected override IInstanceValueParameterSet CreateGreatGrannyParameterSet(IInstanceValueExpressionParameterSet parameters) =>
            new InstanceValueParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        protected override ExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IInstanceValueExpressionParameterSet parameters, Neuron greatGranny) =>
            ProcessorExtensions.CreateValueExpressionParameterSet(externalReferences, greatGranny);
    }
}
