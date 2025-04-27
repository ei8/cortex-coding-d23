using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class IdInstanceValueWriter :
        InstanceValueWriterBase
        <
            IIdInstanceValueParameterSet,
            IIdInstanceValueReader,
            IIdExpressionParameterSet,
            IIdExpressionWriter
        >,
        IIdInstanceValueWriter
    {
        public IdInstanceValueWriter(
            IInstantiatesClassWriter greatGrannyWriter,
            IIdExpressionWriter expressionWriter,
            Readers.Deductive.IIdInstanceValueReader reader,
            IExternalReferenceSet externalReferences
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            externalReferences
        )
        {
        }

        protected override IIdExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences, 
            IIdInstanceValueParameterSet parameters, 
            Neuron greatGranny, 
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreateIdInstanceValueParameterSet(
                externalReferences, 
                parameters.Value, 
                greatGranny,
                parameters.Id
            );
    }
}
