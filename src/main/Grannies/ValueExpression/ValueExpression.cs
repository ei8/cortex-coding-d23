using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class ValueExpression : IValueExpression
    {
        public async Task<IValueExpression> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IValueExpressionParameterSet parameters) =>
            await new ValueExpression().AggregateBuildAsync(
                new IInnerProcess<ValueExpression>[]
                {
                    new InnerProcess<Value, IValue, IValueParameterSet, ValueExpression>(
                        (g) => ValueExpression.CreateValueParameterSet(parameters),
                        (g, r) => r.Value = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        ),
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, ValueExpression>(
                        (g) => ValueExpression.CreateExpressionParameterSet(primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                },
                ensemble,
                primitives,
                (n, r) => r.Neuron = n
                );

        private static IValueParameterSet CreateValueParameterSet(IValueExpressionParameterSet parameters) =>
            new ValueParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.MatchingNeuronProperty,
                parameters.EnsembleRepository,
                parameters.UserId
            );

        public IEnumerable<IGrannyQuery> GetQueries(IPrimitiveSet primitives, IValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<Value, IValue, IValueParameterSet>(
                    (n) => ValueExpression.CreateValueParameterSet(parameters)
                ),
                new GrannyQueryInner<Expression, IExpression, IExpressionParameterSet>(
                    (n) => ValueExpression.CreateExpressionParameterSet(primitives, parameters, n)
                )
            };

        private static ExpressionParameterSet CreateExpressionParameterSet(IPrimitiveSet primitives, IValueExpressionParameterSet parameters, Neuron n) =>
            new ExpressionParameterSet(
                new[] {
                    new UnitParameterSet(
                        n,
                        primitives.Unit
                    ),
                },
                parameters.EnsembleRepository,
                parameters.UserId
            );

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IValueExpressionParameterSet parameters, out IValueExpression result) =>
            (result = new ValueExpression().AggregateTryParse(
                new IInnerProcess<ValueExpression>[]
                {
                    new InnerProcess<Value, IValue, IValueParameterSet, ValueExpression>(
                        (g) => ValueExpression.CreateValueParameterSet(parameters),
                        (g, r) => r.Value = g,
                        ProcessHelper.TryParse
                        ),
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, ValueExpression>(
                        (g) => ValueExpression.CreateExpressionParameterSet(primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                primitives,
                (n, r) => r.Neuron = n
            )) != null;

        public IValue Value { get; private set; }

        public IExpression Expression { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
