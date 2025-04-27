using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ExpressionWriter :
        ExpressionWriterBase<IExpressionReader, IExpressionParameterSet>,
        IExpressionWriter
    {
        public ExpressionWriter(
            IUnitWriter unitWriter, 
            Readers.Deductive.IExpressionReader reader, 
            IExternalReferenceSet externalReferences
        ) : base(unitWriter, reader, externalReferences)
        {
        }

        protected override Neuron CreateGrannyNeuron(IExpressionParameterSet parameters) =>
            Neuron.CreateTransient(null, null, null);
    }
}
