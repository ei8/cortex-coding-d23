using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class InstanceValueWriter :
        InstanceValueWriterBase
        <
            IInstanceValueParameterSet,
            IInstanceValueReader,
            IExpressionParameterSet,
            IExpressionWriter
        >,
        IInstanceValueWriter
    {
        public InstanceValueWriter(
            IInstantiatesClassWriter greatGrannyWriter,
            IExpressionWriter expressionWriter,
            Readers.Deductive.IInstanceValueReader reader,
            IMirrorSet mirrors
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            mirrors
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(
            IMirrorSet mirrors, 
            IInstanceValueParameterSet parameters, 
            Neuron greatGranny, 
            Network network
        ) =>
            Readers.Deductive.ProcessorExtensions.CreateInstanceValueParameterSet(
                mirrors, 
                parameters.Value, 
                greatGranny
            );
    }
}
