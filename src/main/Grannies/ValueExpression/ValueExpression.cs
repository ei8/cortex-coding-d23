using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class ValueExpression : IValueExpression
    {
        private readonly IValue valueProcessor;
        private readonly IExpression expressionProcessor;

        public ValueExpression(IValue valueProcessor, IExpression expressionProcessor)
        {
            this.valueProcessor = valueProcessor;
            this.expressionProcessor = expressionProcessor;
        }

        public async Task<IValueExpression> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IValueExpressionParameterSet parameters) =>
            await new ValueExpression(this.valueProcessor, this.expressionProcessor).AggregateBuildAsync(
                new IInnerProcess<IValueExpression>[]
                {
                    new InnerProcess<IValue, IValueParameterSet, IValueExpression>(
                        this.valueProcessor,
                        (g) => ValueExpression.CreateValueParameterSet(parameters),
                        (g, r) => r.Value = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        ),
                    new InnerProcess<IExpression, IExpressionParameterSet, IValueExpression>(
                        this.expressionProcessor,
                        (g) => ValueExpression.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
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
                new GrannyQueryInner<IValue, IValueParameterSet>(
                    this.valueProcessor,
                    (n) => ValueExpression.CreateValueParameterSet(parameters)
                ),
                new GrannyQueryInner<IExpression, IExpressionParameterSet>(
                    this.expressionProcessor,
                    (n) => ValueExpression.CreateExpressionParameterSet(options.Primitives, parameters, n)
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
            (result = new ValueExpression(this.valueProcessor, this.expressionProcessor).AggregateTryParse(
                new IInnerProcess<IValueExpression>[]
                {
                    new InnerProcess<IValue, IValueParameterSet, IValueExpression>(
                        this.valueProcessor,
                        (g) => ValueExpression.CreateValueParameterSet(parameters),
                        (g, r) => r.Value = g,
                        ProcessHelper.TryParse
                        ),
                    new InnerProcess<IExpression, IExpressionParameterSet, IValueExpression>(
                        this.expressionProcessor,
                        (g) => ValueExpression.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            )) != null;

        public IValue Value { get; set; }

        public IExpression Expression { get; set; }

        public Neuron Neuron { get; set; }
    }
}
