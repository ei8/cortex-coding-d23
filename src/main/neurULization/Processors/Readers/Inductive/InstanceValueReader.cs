using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

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

        protected override IExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IInstanceValueParameterSet parameters, IEnumerable<Neuron> grannyCandidates, Network network) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithGrannyAndType(
                        grannyCandidates.First(),
                        externalReferences.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        grannyCandidates.ElementAt(1),
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
