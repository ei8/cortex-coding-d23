using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ValueExpressionProcessor : IValueExpressionProcessor
    {
        private readonly IValueProcessor valueProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly Readers.Deductive.IValueExpressionProcessor readerProcessor;
        private readonly IPrimitiveSet primitives;

        public ValueExpressionProcessor(
            IValueProcessor valueProcessor, 
            IExpressionProcessor expressionProcessor,
            Readers.Deductive.IValueExpressionProcessor readerProcessor,
            IPrimitiveSet primitives
            )
        {
            this.valueProcessor = valueProcessor;
            this.expressionProcessor = expressionProcessor;
            this.readerProcessor=readerProcessor;
            this.primitives = primitives;
        }

        public IValueExpression Build(Ensemble ensemble, IValueExpressionParameterSet parameters) =>
            new ValueExpression().AggregateBuild(
                ValueExpressionProcessor.CreateGreatGrannies(
                    this.valueProcessor,
                    this.expressionProcessor,
                    parameters,
                    this.primitives
                ),
                new IGreatGrannyProcess<IValueExpression>[]
                {
                    new GreatGrannyProcess<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        ProcessHelper.ParseBuild
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.ParseBuild
                        )
                },
                ensemble
            );

        private static IEnumerable<IGreatGrannyInfo<IValueExpression>> CreateGreatGrannies(
            IValueProcessor valueProcessor,
            IExpressionProcessor expressionProcessor, 
            IValueExpressionParameterSet parameters,
            IPrimitiveSet primitives
        ) =>
            new IGreatGrannyInfo<IValueExpression>[]
            {
                new IndependentGreatGrannyInfo<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                    valueProcessor,
                    () => ValueExpressionProcessor.CreateValueParameterSet(parameters),
                    (g, r) => r.Value = g
                    ),
                new DependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                    expressionProcessor,
                    (g) => ValueExpressionProcessor.CreateExpressionParameterSet(primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                    )
            };

        private static IValueParameterSet CreateValueParameterSet(IValueExpressionParameterSet parameters) =>
            new ValueParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(IPrimitiveSet primitives, IValueExpressionParameterSet parameters, Neuron n) =>
            new ExpressionParameterSet(
                new[] {
                    new UnitParameterSet(
                        n,
                        primitives.Unit
                    ),
                }
            );

        public IGrannyReadProcessor<IValueExpression, IValueExpressionParameterSet> ReadProcessor => this.readerProcessor;
    }
}
