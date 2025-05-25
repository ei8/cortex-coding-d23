using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public abstract class LesserExpressionWriterBase
    <
        TGreatGranny, 
        TGreatGrannyParameterSet, 
        TGreatGrannyWriter, 
        TResult,
        TParameterSet,
        TReader,
        TResultDerived,
        TExpressionParameterSet,
        TExpressionWriter
    > : 
        ExpressionProcessorBase
        <
            TGreatGranny,
            TGreatGrannyParameterSet,
            TGreatGrannyWriter,
            TResult,
            TParameterSet,
            TExpressionParameterSet,
            TExpressionWriter,
            IUnitParameterSet
        >,
        ILesserGrannyWriter<TResult, TParameterSet>
        where TGreatGranny : IGranny
        where TGreatGrannyParameterSet : class, IDeductiveParameterSet
        where TGreatGrannyWriter : IGrannyWriter<TGreatGranny, TGreatGrannyParameterSet>
        where TResult : IExpressionGranny, ILesserGranny<TGreatGranny>
        where TParameterSet : IDeductiveParameterSet
        where TReader : IGrannyReader<TResult, TParameterSet>
        where TResultDerived : TResult, new()
        where TExpressionParameterSet : IExpressionParameterSet
        where TExpressionWriter : ILesserGrannyWriter<IExpression, TExpressionParameterSet>
    {
        private readonly TReader reader;
        private readonly IMirrorSet mirrors;

        protected LesserExpressionWriterBase
        (
            TGreatGrannyWriter greatGrannyWriter,
            TExpressionWriter expressionWriter,
            TReader reader,
            IMirrorSet mirrors
        ) : 
            base(greatGrannyWriter, expressionWriter)
        {
            AssertionConcern.AssertArgumentNotNull(reader, nameof(reader));
            AssertionConcern.AssertArgumentNotNull(mirrors, nameof(mirrors));

            this.reader = reader;
            this.mirrors = mirrors;
        }

        public bool TryBuild(Network network, TParameterSet parameters, out TResult result) =>
            this.TryBuildAggregate(
                () => new TResultDerived(),
                parameters,
                new IGreatGrannyProcess<TResult>[]
                {
                    new GreatGrannyProcess<TGreatGranny, TGreatGrannyWriter, TGreatGrannyParameterSet, TResult>(
                        ProcessHelper.TryParseBuild
                        ),
                    new GreatGrannyProcess<IExpression, TExpressionWriter, TExpressionParameterSet, TResult>(
                        ProcessHelper.TryParseBuild
                        )
                },
                network,
                this.mirrors,
                out result
            );

        public IGrannyReader<TResult, TParameterSet> Reader => this.reader;
    }
}
