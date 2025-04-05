using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class InstanceValueWriter :
        ExpressionWriterBase<
            IInstantiatesClass,
            IInstantiatesClassParameterSet,
            IInstantiatesClassWriter,
            IInstanceValue,
            IInstanceValueReader,
            IInstanceValueParameterSet,
            InstanceValue
        >,
        IInstanceValueWriter
    {
        public InstanceValueWriter(
            IInstantiatesClassWriter greatGrannyWriter,
            IExpressionWriter expressionWriter,
            Readers.Deductive.IInstanceValueReader reader,
            IExternalReferenceSet externalReferences
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            externalReferences
        )
        {
        }

        protected override IExpressionParameterSet CreateExpressionParameterSet(IExternalReferenceSet externalReferences, IInstanceValueParameterSet parameters, Neuron greatGranny, Network network) =>
            Readers.Deductive.ProcessorExtensions.CreateInstanceValueParameterSet(externalReferences, parameters.Value, greatGranny);

        protected override IInstantiatesClassParameterSet CreateGreatGrannyParameterSet(IInstanceValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );
    }
}
