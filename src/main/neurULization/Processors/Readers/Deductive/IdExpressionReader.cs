namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class IdExpressionReader : 
        ExpressionReaderBase<IIdExpressionParameterSet>, 
        IIdExpressionReader
    {
        public IdExpressionReader(IUnitReader unitReader, IMirrorSet mirrors) : 
            base(unitReader, mirrors)
        {
        }
    }
}
