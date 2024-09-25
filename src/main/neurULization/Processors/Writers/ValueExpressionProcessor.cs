using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IValueExpression> BuildAsync(Ensemble ensemble, IValueExpressionParameterSet parameters) =>
            await new ValueExpression().AggregateBuildAsync(
                ValueExpressionProcessor.CreateGreatGrannies(
                    this.valueProcessor,
                    this.expressionProcessor,
                    parameters,
                    this.primitives
                ),
                new IGreatGrannyProcessAsync<IValueExpression>[]
                {
                    new GreatGrannyProcessAsync<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        ),
                    new GreatGrannyProcessAsync<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
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
