using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public abstract class ExpressionReaderBase<
        TGreatGranny,
        TGreatGrannyParameterSet,
        TGreatGrannyReader,
        TResult,
        TParameterSet,
        TResultDerived
    >
        where TGreatGranny : IGranny
        where TGreatGrannyParameterSet : IInductiveParameterSet
        where TGreatGrannyReader : IGrannyReader<TGreatGranny, TGreatGrannyParameterSet>
        where TResult : IExpressionGranny, ILesserGranny<TGreatGranny>
        where TParameterSet : IInductiveParameterSet
        where TResultDerived : TResult, new()
    {
        private readonly TGreatGrannyReader greatGrannyReader;
        private readonly IExpressionReader expressionReader;
        private readonly IExternalReferenceSet externalReferences;
        private readonly IAggregateParser aggregateParser;

        protected ExpressionReaderBase(
            TGreatGrannyReader greatGrannyReader,
            IExpressionReader expressionReader,
            IExternalReferenceSet externalReferences,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(greatGrannyReader, nameof(greatGrannyReader));
            AssertionConcern.AssertArgumentNotNull(expressionReader, nameof(expressionReader));
            AssertionConcern.AssertArgumentNotNull(externalReferences, nameof(externalReferences));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.greatGrannyReader = greatGrannyReader;
            this.expressionReader = expressionReader;
            this.externalReferences = externalReferences;
            this.aggregateParser = aggregateParser;
        }

        protected abstract TGreatGrannyParameterSet CreateGreatGrannyParameterSet(
            TParameterSet parameters,
            Neuron grannyCandidate
        );

        protected abstract IExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            TParameterSet parameters,
            Neuron grannyCandidate,
            Network network
        );

        protected virtual Guid GetValueUnitTypeId(
            IExternalReferenceSet externalReferences
        ) => externalReferences.Unit.Id;

        protected IGreatGrannyInfoSuperset<TResult> CreateGreatGrannies(
            TGreatGrannyReader greatGrannyReader,
            IExpressionReader expressionReader,
            TParameterSet parameters,
            Network network,
            IExternalReferenceSet externalReferences
        ) =>
            GreatGrannyInfoSuperset<TResult>.Create(
                new GreatGrannyInfoSet<TResult>[] {
                    ProcessHelper.CreateGreatGrannyCandidateSet(
                        network,
                        parameters.Granny,
                        gc => new InductiveIndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, TResult>(
                            gc,
                            expressionReader,
                            () => this.CreateExpressionParameterSet(externalReferences, parameters, gc, network),
                            (g, r) => r.Expression = g
                        ),
                        new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, TResult>(
                            ProcessHelper.TryParse
                        )
                    ),
                    new GreatGrannyInfoSet<TResult>(
                        new IGreatGrannyInfo<TResult>[]
                        {
                            new DependentGreatGrannyInfo<TGreatGranny, TGreatGrannyReader, TGreatGrannyParameterSet, TResult>(
                                greatGrannyReader,
                                g => this.CreateGreatGrannyParameterSet(
                                    parameters,
                                    ((IExpression) g).Units.GetValueUnitGranniesByTypeId(this.GetValueUnitTypeId(externalReferences)).Single().Value
                                    ),
                                (g, r) => r.GreatGranny = g
                            )
                        },
                        new GreatGrannyProcess<TGreatGranny, TGreatGrannyReader , TGreatGrannyParameterSet, TResult>(
                            ProcessHelper.TryParse
                        )
                    )
                }
            );

        public bool TryParse(Network network, TParameterSet parameters, out TResult result) =>
            this.aggregateParser.TryParse<TResultDerived, TResult>(
                parameters.Granny,
                this.CreateGreatGrannies(
                    this.greatGrannyReader,
                    this.expressionReader,
                    parameters,
                    network,
                    this.externalReferences
                ),
                network,
                out result
            );
    }
}
