using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ValueExpressionProcessor : IValueExpressionProcessor
    {
        private readonly IValueProcessor valueProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly IPrimitiveSet primitives;

        public ValueExpressionProcessor(
            IValueProcessor valueProcessor, 
            IExpressionProcessor expressionProcessor, 
            IPrimitiveSet primitives
        )
        {
            this.valueProcessor = valueProcessor;
            this.expressionProcessor = expressionProcessor;
            this.primitives = primitives;
        }

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

        public IEnumerable<IGrannyQuery> GetQueries(IValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IValue, IValueProcessor, IValueParameterSet>(
                    valueProcessor,
                    (n) => CreateValueParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    expressionProcessor,
                    (n) => CreateExpressionParameterSet(this.primitives, parameters, n.Last().Neuron)
                )
            };

        private static ExpressionParameterSet CreateExpressionParameterSet(IPrimitiveSet primitives, IValueExpressionParameterSet parameters, Neuron n) =>
            new ExpressionParameterSet(
                new[] {
                    new UnitParameterSet(
                        n,
                        primitives.Unit
                    ),
                }
            );

        public bool TryParse(Ensemble ensemble, IValueExpressionParameterSet parameters, out IValueExpression result) =>
            new ValueExpression().AggregateTryParse(
                ValueExpressionProcessor.CreateGreatGrannies(
                    this.valueProcessor,
                    this.expressionProcessor,
                    parameters,
                    this.primitives
                ),
                new IGreatGrannyProcess<IValueExpression>[]
                {
                    new GreatGrannyProcess<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                out result
            );
    }
}
