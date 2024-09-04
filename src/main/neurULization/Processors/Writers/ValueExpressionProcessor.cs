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

        public ValueExpressionProcessor(
            IValueProcessor valueProcessor, 
            IExpressionProcessor expressionProcessor,
            Readers.Deductive.IValueExpressionProcessor readerProcessor
            )
        {
            this.valueProcessor = valueProcessor;
            this.expressionProcessor = expressionProcessor;
            this.readerProcessor=readerProcessor;
        }

        public async Task<IValueExpression> BuildAsync(Ensemble ensemble, Id23neurULizerWriteOptions options, IValueExpressionParameterSet parameters) =>
            await new ValueExpression().AggregateBuildAsync(
                CreateGreatGrannies(options, parameters),
                new IGreatGrannyProcessAsync<IValueExpression>[]
                {
                    new GreatGrannyProcessAsync<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        ),
                    new GreatGrannyProcessAsync<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                },
                ensemble,
                options
            );

        private IEnumerable<IGreatGrannyInfo<IValueExpression>> CreateGreatGrannies(Id23neurULizerWriteOptions options, IValueExpressionParameterSet parameters) =>
            new IGreatGrannyInfo<IValueExpression>[]
            {
                new IndependentGreatGrannyInfo<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                    valueProcessor,
                    () => CreateValueParameterSet(parameters),
                    (g, r) => r.Value = g
                    ),
                new DependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                    expressionProcessor,
                    (g) => CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                    )
            };

        private static IValueParameterSet CreateValueParameterSet(IValueExpressionParameterSet parameters) =>
            new ValueParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(PrimitiveSet primitives, IValueExpressionParameterSet parameters, Neuron n) =>
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
