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
            IMirrorSet mirrors
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            mirrors
        )
        {
        }

        protected override IIdExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors, 
            IIdInstanceValueParameterSet parameters, 
            Neuron greatGranny, 
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreateIdInstanceValueParameterSet(
                mirrors, 
                parameters.Value, 
                greatGranny,
                parameters.Id
            );
    }
}
