using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors
{
    public abstract class ExpressionProcessorBase
    <
        TGreatGranny,
        TGreatGrannyParameterSet,
        TGreatGrannyProcessor,
        TGranny,
        TParameterSet,
        TExpressionParameterSet,
        TExpressionProcessor,
        TUnitParameterSet
    > : 
        IGrannyProcessor<TGranny, TParameterSet>
        where TGreatGranny : IGranny
        where TGreatGrannyParameterSet : IParameterSet
        where TGreatGrannyProcessor : IGrannyProcessor<TGreatGranny, TGreatGrannyParameterSet>
        where TGranny : IExpressionGranny, ILesserGranny<TGreatGranny>
        where TParameterSet : IParameterSet
        where TExpressionParameterSet : IExpressionParameterSetCore<TUnitParameterSet>
        where TExpressionProcessor : IGrannyProcessor<IExpression, TExpressionParameterSet>
        where TUnitParameterSet : IUnitParameterSetCore
    {
        protected readonly TGreatGrannyProcessor greatGrannyProcessor;
        protected readonly TExpressionProcessor expressionProcessor;

        protected ExpressionProcessorBase(
            TGreatGrannyProcessor greatGrannyProcessor,
            TExpressionProcessor expressionProcessor
        )
        {
            AssertionConcern.AssertArgumentNotNull(greatGrannyProcessor, nameof(greatGrannyProcessor));
            AssertionConcern.AssertArgumentNotNull(expressionProcessor, nameof(expressionProcessor));

            this.greatGrannyProcessor = greatGrannyProcessor;
            this.expressionProcessor = expressionProcessor;
        }

        protected abstract TGreatGrannyParameterSet CreateGreatGrannyParameterSet(TParameterSet parameters);

        protected abstract TExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            TParameterSet parameters,
            Neuron greatGranny,
            Network network
        );

        public bool TryCreateGreatGrannies(
            TParameterSet parameters,
            Network network,
            IExternalReferenceSet externalReferences,            
            out IEnumerable<IGreatGrannyInfo<TGranny>> result
        ) => this.TryCreateGreatGranniesCore(
            delegate (out bool bResult) {
                bResult = true;
                var coreBResult = true;
                var coreResult = new IGreatGrannyInfo<TGranny>[]
                {
                    new IndependentGreatGrannyInfo<TGreatGranny, TGreatGrannyProcessor, TGreatGrannyParameterSet, TGranny>(
                        greatGrannyProcessor,
                        () => CreateGreatGrannyParameterSet(parameters),
                        (g, r) => r.GreatGranny = g
                        ),
                    new DependentGreatGrannyInfo<IExpression, TExpressionProcessor, TExpressionParameterSet, TGranny>(
                        expressionProcessor,
                        (g) => CreateExpressionParameterSet(externalReferences, parameters, g.Neuron, network),
                        (g, r) => r.Expression = g
                        )
                };
                bResult = coreBResult;
                return coreResult;
            },
            out result
        );
    }
}
