using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
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
    > : ILesserGrannyReader<TResult, TParameterSet>
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
            IEnumerable<Neuron> grannyCandidates,
            Network network
        );

        protected virtual Guid GetValueUnitTypeId(
            IExternalReferenceSet externalReferences
        ) => externalReferences.Unit.Id;

        public bool TryCreateGreatGrannies(
            TParameterSet parameters,
            Network network,
            IExternalReferenceSet externalReferences,
            out IGreatGrannyInfoSuperset<TResult> result
        ) => this.TryCreateGreatGranniesCore(
            delegate (out bool bResult) {
                bResult = true;
                var coreBResult = true;
                var coreResult = ProcessHelper.CreatePermutatedGreatGrannyInfoSuperset(
                    network,
                    parameters.Granny,
                    gcs => new InductiveIndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, TResult>(
                        gcs,
                        this.expressionReader,
                        () => this.CreateExpressionParameterSet(externalReferences, parameters, gcs, network),
                        (g, r) => r.Expression = g
                    ),
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, TResult>(
                        ProcessHelper.TryParse
                    )
                ).Append(
                    new GreatGrannyInfoSet<TResult>(
                        new[]
                        {
                            new DependentGreatGrannyInfo<TGreatGranny, TGreatGrannyReader, TGreatGrannyParameterSet, TResult>(
                                this.greatGrannyReader,
                                g => {
                                    if (
                                        coreBResult = ((IExpression) g).TryGetValueUnitGrannyByTypeId(
                                            this.GetValueUnitTypeId(externalReferences), 
                                            out IUnit vuResult
                                        )
                                    )
                                        return this.CreateGreatGrannyParameterSet(
                                            parameters,
                                            vuResult.Value
                                        );
                                    else
                                        return default;
                                },
                                (g, r) => r.GreatGranny = g
                            )
                        },
                        new GreatGrannyProcess<TGreatGranny, TGreatGrannyReader, TGreatGrannyParameterSet, TResult>(
                            ProcessHelper.TryParse
                        )
                    )
                );
                bResult = coreBResult;
                return coreResult;
            },
            out result
        );

        public bool TryParse(Network network, TParameterSet parameters, out TResult result) =>
            this.aggregateParser.TryParse<TResultDerived, TResult, TParameterSet>(
                parameters.Granny,
                this,
                parameters,
                network,
                this.externalReferences,
                out result
            );
    }
}
