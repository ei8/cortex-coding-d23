namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ExpressionReader : 
        ExpressionReaderBase<IExpressionParameterSet>,
        IExpressionReader
    {
        public ExpressionReader(IUnitReader unitReader, IMirrorSet mirrors) :
            base(unitReader, mirrors)
        {
        }
    }
}
