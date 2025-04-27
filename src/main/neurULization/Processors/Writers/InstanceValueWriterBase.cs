using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public abstract class InstanceValueWriterBase
    <
        TInstanceValueParameterSet, 
        TInstanceValueReader,
        TExpressionParameterSet,
        TExpressionWriter
    > :
        LesserExpressionWriterBase
        <
            IInstantiatesClass,
            IInstantiatesClassParameterSet,
            IInstantiatesClassWriter,
            IInstanceValue,
            TInstanceValueParameterSet,
            TInstanceValueReader,
            InstanceValue,
            TExpressionParameterSet,
            TExpressionWriter
        >
        where TInstanceValueReader : IGrannyReader<IInstanceValue, TInstanceValueParameterSet>
        where TInstanceValueParameterSet : IInstanceValueParameterSet
        where TExpressionWriter : ILesserGrannyWriter<IExpression, TExpressionParameterSet>
        where TExpressionParameterSet : IExpressionParameterSet
    {
        public InstanceValueWriterBase(
            IInstantiatesClassWriter greatGrannyWriter,
            TExpressionWriter expressionWriter,
            TInstanceValueReader reader,
            IExternalReferenceSet externalReferences
        ) : base(
            greatGrannyWriter,
            expressionWriter,
            reader,
            externalReferences
        )
        {
        }

        protected override IInstantiatesClassParameterSet CreateGreatGrannyParameterSet(TInstanceValueParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );
    }
}
