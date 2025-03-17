using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public abstract class ExpressionWriterBase<
        TGreatGranny, 
        TGreatGrannyParameterSet, 
        TGreatGrannyWriter, 
        TResult,
        TReader,
        TParameterSet,
        TResultDerived
    > : ExpressionProcessorBase<
        TGreatGranny,
        TGreatGrannyParameterSet,
        TGreatGrannyWriter,
        TResult,
        TParameterSet
    >
        where TGreatGranny : IGranny
        where TGreatGrannyParameterSet : class, IDeductiveParameterSet
        where TGreatGrannyWriter : IGrannyWriter<TGreatGranny, TGreatGrannyParameterSet>
        where TResult : IExpressionGranny, ILesserGranny<TGreatGranny>
        where TReader : IGrannyReader<TResult, TParameterSet>
        where TParameterSet : IDeductiveParameterSet
        where TResultDerived : TResult, new()
    {
        private readonly TGreatGrannyWriter greatGrannyWriter;
        private readonly IExpressionWriter expressionWriter;
        private readonly TReader reader;
        private readonly IExternalReferenceSet externalReferences;

        protected ExpressionWriterBase(
            TGreatGrannyWriter greatGrannyWriter,
            IExpressionWriter expressionWriter,
            TReader reader,
            IExternalReferenceSet externalReferences
        )
        {
            this.greatGrannyWriter = greatGrannyWriter;
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.externalReferences = externalReferences;
        }

        public TResult Build(Network network, TParameterSet parameters) =>
            new TResultDerived().AggregateBuild(
                this.CreateGreatGrannies(
                    this.greatGrannyWriter,
                    this.expressionWriter,
                    parameters,
                    this.externalReferences
                ),
                new IGreatGrannyProcess<TResult>[]
                {
                    new GreatGrannyProcess<TGreatGranny, TGreatGrannyWriter, TGreatGrannyParameterSet, TResult>(
                        ProcessHelper.ParseBuild
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionWriter, IExpressionParameterSet, TResult>(
                        ProcessHelper.ParseBuild
                        )
                },
                network
            );

        public IGrannyReader<TResult, TParameterSet> Reader => this.reader;
    }
}
