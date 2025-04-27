using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyInstanceValueExpressionWriter : 
        LesserExpressionWriterBase
        <
            IInstanceValueExpression,
            IInstanceValueExpressionParameterSet,
            IInstanceValueExpressionWriter,
            IPropertyInstanceValueExpression,
            IPropertyInstanceValueExpressionParameterSet,
            IPropertyInstanceValueExpressionReader,
            PropertyInstanceValueExpression,
            IExpressionParameterSet,
            IExpressionWriter
        >, 
        IPropertyInstanceValueExpressionWriter
    {
        public PropertyInstanceValueExpressionWriter(
            IInstanceValueExpressionWriter greatGrannyWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyInstanceValueExpressionReader reader,
            IExternalReferenceSet externalReferences
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            externalReferences
        )
        {
        }

        protected override IInstanceValueExpressionParameterSet CreateGreatGrannyParameterSet(IPropertyInstanceValueExpressionParameterSet parameters) =>
            new InstanceValueExpressionParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences, 
            IPropertyInstanceValueExpressionParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreatePropertyValueExpressionParameterSet(externalReferences, greatGranny);
    }
}
