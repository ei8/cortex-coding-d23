using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors
{
    public abstract class ExpressionProcessorBase<
        TGreatGranny,
        TGreatGrannyParameterSet,
        TGreatGrannyProcessor,
        TResult,
        TParameterSet,
        TExpressionParameterSet,
        TExpressionProcessor,
        TUnitParameterSet
    >
        where TGreatGranny : IGranny
        where TGreatGrannyParameterSet : IParameterSet
        where TGreatGrannyProcessor : IGrannyProcessor<TGreatGranny, TGreatGrannyParameterSet>
        where TResult : IExpressionGranny, ILesserGranny<TGreatGranny>
        where TParameterSet : IParameterSet
        where TExpressionParameterSet : IExpressionParameterSetCore<TUnitParameterSet>
        where TExpressionProcessor : IGrannyProcessor<IExpression, TExpressionParameterSet>
        where TUnitParameterSet : IUnitParameterSetCore
    {
        protected abstract TGreatGrannyParameterSet CreateGreatGrannyParameterSet(TParameterSet parameters);

        protected abstract TExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            TParameterSet parameters,
            Neuron greatGranny,
            Network network
        );

        protected IEnumerable<IGreatGrannyInfo<TResult>> CreateGreatGrannies(
            TGreatGrannyProcessor greatGrannyProcessor,
            TExpressionProcessor expressionProcessor,
            TParameterSet parameters,
            IExternalReferenceSet externalReferences,
            Network network
        ) =>
            new IGreatGrannyInfo<TResult>[]
            {
                new IndependentGreatGrannyInfo<TGreatGranny, TGreatGrannyProcessor, TGreatGrannyParameterSet, TResult>(
                    greatGrannyProcessor,
                    () => CreateGreatGrannyParameterSet(parameters),
                    (g, r) => r.GreatGranny = g
                    ),
                new DependentGreatGrannyInfo<IExpression, TExpressionProcessor, TExpressionParameterSet, TResult>(
                    expressionProcessor,
                    (g) => CreateExpressionParameterSet(externalReferences, parameters, g.Neuron, network),
                    (g, r) => r.Expression = g
                    )
            };
    }
}
