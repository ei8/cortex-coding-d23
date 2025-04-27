namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class IdExpressionReader : 
        ExpressionReaderBase<IIdExpressionParameterSet>, 
        IIdExpressionReader
    {
        public IdExpressionReader(IUnitReader unitReader, IExternalReferenceSet externalReferences) : 
            base(unitReader, externalReferences)
        {
        }
    }
}
