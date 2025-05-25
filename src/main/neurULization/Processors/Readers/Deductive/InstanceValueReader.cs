namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class InstanceValueReader : 
        InstanceValueReaderBase<IInstanceValueParameterSet>,
        IInstanceValueReader
    {
        public InstanceValueReader(
            IInstantiatesClassReader greatGrannyReader,
            IExpressionReader expressionReader,
            IMirrorSet mirrors
        ) : base(
            greatGrannyReader,
            expressionReader,
            mirrors
        )
        {
        }
    }
}
