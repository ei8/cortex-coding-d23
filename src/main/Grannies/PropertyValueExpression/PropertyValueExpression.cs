using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyValueExpression : IPropertyValueExpression
    {
        public async Task<IPropertyValueExpression> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IPropertyValueExpressionParameterSet parameters) =>
            await new PropertyValueExpression().AggregateBuildAsync(
                new IInnerProcess<PropertyValueExpression>[]
                {
                    new InnerProcess<ValueExpression, IValueExpression, IValueExpressionParameterSet, PropertyValueExpression>(
                        (g) => PropertyValueExpression.CreateValueExpressionParameterSet(parameters),
                        (g, r) => r.ValueExpression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    ),
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, PropertyValueExpression>(
                        (g) => PropertyValueExpression.CreateExpressionParameterSet(primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                primitives,
                parameters.EnsembleRepository,
                parameters.UserId,
                (n, r) => r.Neuron = n
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(IPrimitiveSet primitives, IPropertyValueExpressionParameterSet parameters, Neuron neuron) =>
            new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        neuron,
                        primitives.Unit
                    ),
                    new UnitParameterSet(
                        primitives.Of,
                        primitives.Case
                    )
                },
                parameters.EnsembleRepository,
                parameters.UserId
            );

        private static IValueExpressionParameterSet CreateValueExpressionParameterSet(IPropertyValueExpressionParameterSet parameters) =>
            new ValueExpressionParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy,
                parameters.EnsembleRepository,
                parameters.UserId
            );
        

        public IEnumerable<IGrannyQuery> GetQueries(IPrimitiveSet primitives, IPropertyValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<ValueExpression, IValueExpression, IValueExpressionParameterSet>(
                    (n) => PropertyValueExpression.CreateValueExpressionParameterSet(parameters)
                ),
                new GrannyQueryInner<Expression, IExpression, IExpressionParameterSet>(
                    (n) => PropertyValueExpression.CreateExpressionParameterSet(primitives, parameters, n)
                )
            };

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IPropertyValueExpressionParameterSet parameters, out IPropertyValueExpression result) =>
            (result = new PropertyValueExpression().AggregateTryParse(
                new IInnerProcess<PropertyValueExpression>[]
                {
                    new InnerProcess<ValueExpression, IValueExpression, IValueExpressionParameterSet, PropertyValueExpression>(
                        (g) => PropertyValueExpression.CreateValueExpressionParameterSet(parameters),
                        (g, r) => r.ValueExpression = g,
                        ProcessHelper.TryParse
                        ),
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, PropertyValueExpression>(
                        (g) => PropertyValueExpression.CreateExpressionParameterSet(primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                primitives,
                parameters.EnsembleRepository,
                parameters.UserId,
                (n, r) => r.Neuron = n
            )) != null;

        public IValueExpression ValueExpression { get; private set; }

        public IExpression Expression { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
