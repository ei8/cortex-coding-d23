using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyValueExpressionWriter : 
        LesserExpressionWriterBase
        <
            IValueExpression,
            IValueExpressionParameterSet,
            IValueExpressionWriter,
            IPropertyValueExpression,
            IPropertyValueExpressionParameterSet,
            IPropertyValueExpressionReader,
            PropertyValueExpression,
            IExpressionParameterSet,
            IExpressionWriter
        >, 
        IPropertyValueExpressionWriter
    {
        public PropertyValueExpressionWriter(
            IValueExpressionWriter greatGrannyWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyValueExpressionReader reader,
            IExternalReferenceSet externalReferences
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            externalReferences
        )
        {
        }

        protected override IValueExpressionParameterSet CreateGreatGrannyParameterSet(IPropertyValueExpressionParameterSet parameters) =>
            new ValueExpressionParameterSet(
                parameters.Value
            );

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences, 
            IPropertyValueExpressionParameterSet parameters, 
            Neuron greatGranny,
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreatePropertyValueExpressionParameterSet(externalReferences, greatGranny);
    }
}
