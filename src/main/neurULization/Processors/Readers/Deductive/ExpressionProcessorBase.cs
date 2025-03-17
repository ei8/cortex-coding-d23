using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public abstract class ExpressionProcessorBase<
        TGreatGranny,
        TGreatGrannyParameterSet,
        TGreatGrannyProcessor,
        TResult,
        TParameterSet
    >
        where TGreatGranny : IGranny
        where TGreatGrannyParameterSet : IDeductiveParameterSet
        where TGreatGrannyProcessor : IGrannyProcessor<TGreatGranny, TGreatGrannyParameterSet>
        where TResult : IExpressionGranny, ILesserGranny<TGreatGranny>
        where TParameterSet : IDeductiveParameterSet
    {
        protected abstract TGreatGrannyParameterSet CreateGreatGrannyParameterSet(TParameterSet parameters);

        protected abstract ExpressionParameterSet CreateExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            TParameterSet parameters,
            Neuron greatGranny
        );

        protected IEnumerable<IGreatGrannyInfo<TResult>> CreateGreatGrannies(
            TGreatGrannyProcessor greatGrannyProcessor,
            IGrannyProcessor<IExpression, IExpressionParameterSet> expressionProcessor,
            TParameterSet parameters,
            IExternalReferenceSet externalReferences
        ) =>
            new IGreatGrannyInfo<TResult>[]
            {
                new IndependentGreatGrannyInfo<TGreatGranny, TGreatGrannyProcessor, TGreatGrannyParameterSet, TResult>(
                    greatGrannyProcessor,
                    () => this.CreateGreatGrannyParameterSet(parameters),
                    (g, r) => r.TypedGreatGranny = g
                    ),
                new DependentGreatGrannyInfo<IExpression, IGrannyProcessor<IExpression, IExpressionParameterSet>, IExpressionParameterSet, TResult>(
                    expressionProcessor,
                    (g) => this.CreateExpressionParameterSet(externalReferences, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                    )
            };
    }
}
