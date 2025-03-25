using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstanceValueReader :
        ExpressionReaderBase<
            IInstantiatesClass,
            IInstantiatesClassParameterSet,
            IInstantiatesClassReader,
            IInstanceValue,
            IInstanceValueParameterSet,
            InstanceValue
        >,
        IInstanceValueReader
    {
        public InstanceValueReader(
            IInstantiatesClassReader greatGrannyReader,
            IExpressionReader expressionReader,
            IExternalReferenceSet externalReferences,
            IAggregateParser aggregateParser
        ) : base(
            greatGrannyReader,
            expressionReader,
            externalReferences,
            aggregateParser
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IInstanceValueParameterSet parameters, Neuron grannyCandidate, Network network) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithGrannyAndType(
                        grannyCandidate,
                        externalReferences.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        grannyCandidate,
                        externalReferences.NominalSubject
                    )
                }
            );

        protected override IInstantiatesClassParameterSet CreateGreatGrannyParameterSet(IInstanceValueParameterSet parameters, Neuron grannyCandidate) =>
            new InstantiatesClassParameterSet(
                grannyCandidate,
                parameters.Class
            );
    }
}
