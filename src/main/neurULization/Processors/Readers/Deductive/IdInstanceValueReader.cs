namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class IdInstanceValueReader : 
        InstanceValueReaderBase<IIdInstanceValueParameterSet>,
        IIdInstanceValueReader
    {
        public IdInstanceValueReader(
            IInstantiatesClassReader greatGrannyReader,
            IExpressionReader expressionReader,
            IExternalReferenceSet externalReferences
        ) : base (
            greatGrannyReader, 
            expressionReader, 
            externalReferences
        )
        {
        }
    }
}
