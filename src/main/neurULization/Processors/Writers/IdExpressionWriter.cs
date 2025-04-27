using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class IdExpressionWriter :
        ExpressionWriterBase<IIdExpressionReader, IIdExpressionParameterSet>,
        IIdExpressionWriter
    {
        public IdExpressionWriter(
            IUnitWriter unitWriter, 
            Readers.Deductive.IIdExpressionReader reader, 
            IExternalReferenceSet externalReferences
        ) : base(unitWriter, reader, externalReferences)
        {
        }

        protected override Neuron CreateGrannyNeuron(IIdExpressionParameterSet parameters) =>
            Neuron.CreateTransient(parameters.Id, null, null, null);
    }
}
