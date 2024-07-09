using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class ValueExpression : IValueExpression
    {
        public async Task<IValueExpression> BuildAsync(Ensemble ensemble, Id23neurULizationOptions options, IValueExpressionParameterSet parameters) =>
            await new ValueExpression().AggregateBuildAsync(
                new IInnerProcess<ValueExpression>[]
                {
                    new InnerProcess<Value, IValue, IValueParameterSet, ValueExpression>(
                        (g) => ValueExpression.CreateValueParameterSet(parameters),
                        (g, r) => r.Value = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        ),
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, ValueExpression>(
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

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizationOptions options, IValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<Value, IValue, IValueParameterSet>(
                    (n) => ValueExpression.CreateValueParameterSet(parameters)
                ),
                new GrannyQueryInner<Expression, IExpression, IExpressionParameterSet>(
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

        public bool TryParse(Ensemble ensemble, Id23neurULizationOptions options, IValueExpressionParameterSet parameters, out IValueExpression result) =>
            (result = new ValueExpression().AggregateTryParse(
                new IInnerProcess<ValueExpression>[]
                {
                    new InnerProcess<Value, IValue, IValueParameterSet, ValueExpression>(
                        (g) => ValueExpression.CreateValueParameterSet(parameters),
                        (g, r) => r.Value = g,
                        ProcessHelper.TryParse
                        ),
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, ValueExpression>(
                        (g) => ValueExpression.CreateExpressionParameterSet(options.Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options,
                (n, r) => r.Neuron = n
            )) != null;

        public IValue Value { get; private set; }

        public IExpression Expression { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
