using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class ValueExpressionProcessor : IValueExpressionProcessor
    {
        private readonly IValueProcessor valueProcessor;
        private readonly IExpressionProcessor expressionProcessor;

        public ValueExpressionProcessor(IValueProcessor valueProcessor, IExpressionProcessor expressionProcessor)
        {
            this.valueProcessor = valueProcessor;
            this.expressionProcessor = expressionProcessor;
        }

        public async Task<IValueExpression> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IValueExpressionParameterSet parameters) =>
            await new ValueExpression().AggregateBuildAsync(
                new IInnerProcess<IValueExpression>[]
                {
                    new InnerProcess<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        this.valueProcessor,
                        (g) => ValueExpressionProcessor.CreateValueParameterSet(parameters),
                        (g, r) => r.Value = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        ),
                    new InnerProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        this.expressionProcessor,
                        (g) => ValueExpressionProcessor.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            );

        private static IValueParameterSet CreateValueParameterSet(IValueExpressionParameterSet parameters) =>
            new ValueParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerOptions options, IValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<IValue, IValueProcessor, IValueParameterSet>(
                    this.valueProcessor,
                    (n) => ValueExpressionProcessor.CreateValueParameterSet(parameters)
                ),
                new GrannyQueryInner<IExpression, IExpressionProcessor, IExpressionParameterSet>(
                    this.expressionProcessor,
                    (n) => ValueExpressionProcessor.CreateExpressionParameterSet(options.Primitives, parameters, n)
                )
            };

        private static ExpressionParameterSet CreateExpressionParameterSet(PrimitiveSet primitives, IValueExpressionParameterSet parameters, Neuron n) =>
            new ExpressionParameterSet(
                new[] {
                    new UnitParameterSet(
                        n,
                        primitives.Unit
                    ),
                }
            );

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IValueExpressionParameterSet parameters, out IValueExpression result) =>
            (result = new ValueExpression().AggregateTryParse(
                new IInnerProcess<IValueExpression>[]
                {
                    new InnerProcess<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        this.valueProcessor,
                        (g) => ValueExpressionProcessor.CreateValueParameterSet(parameters),
                        (g, r) => r.Value = g,
                        ProcessHelper.TryParse
                        ),
                    new InnerProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        this.expressionProcessor,
                        (g) => ValueExpressionProcessor.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            )) != null;
    }
}
