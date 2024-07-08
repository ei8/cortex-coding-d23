using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class PropertyValueExpression : IPropertyValueExpression
    {
        public async Task<IPropertyValueExpression> BuildAsync(Ensemble ensemble, IneurULizationOptions options, IPropertyValueExpressionParameterSet parameters) =>
            await new PropertyValueExpression().AggregateBuildAsync(
                new IInnerProcess<PropertyValueExpression>[]
                {
                    new InnerProcess<ValueExpression, IValueExpression, IValueExpressionParameterSet, PropertyValueExpression>(
                        (g) => PropertyValueExpression.CreateValueExpressionParameterSet(parameters),
                        (g, r) => r.ValueExpression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    ),
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, PropertyValueExpression>(
                        (g) => PropertyValueExpression.CreateExpressionParameterSet(options.ToInternal().Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                    )
                },
                ensemble,
                options.ToInternal(),
                (n, r) => r.Neuron = n
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(PrimitiveSet primitives, IPropertyValueExpressionParameterSet parameters, Neuron neuron) =>
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
                }
            );

        private static IValueExpressionParameterSet CreateValueExpressionParameterSet(IPropertyValueExpressionParameterSet parameters) =>
            new ValueExpressionParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );
        

        public IEnumerable<IGrannyQuery> GetQueries(IneurULizationOptions options, IPropertyValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<ValueExpression, IValueExpression, IValueExpressionParameterSet>(
                    (n) => PropertyValueExpression.CreateValueExpressionParameterSet(parameters)
                ),
                new GrannyQueryInner<Expression, IExpression, IExpressionParameterSet>(
                    (n) => PropertyValueExpression.CreateExpressionParameterSet(options.ToInternal().Primitives, parameters, n)
                )
            };

        public bool TryParse(Ensemble ensemble, IneurULizationOptions options, IPropertyValueExpressionParameterSet parameters, out IPropertyValueExpression result) =>
            (result = new PropertyValueExpression().AggregateTryParse(
                new IInnerProcess<PropertyValueExpression>[]
                {
                    new InnerProcess<ValueExpression, IValueExpression, IValueExpressionParameterSet, PropertyValueExpression>(
                        (g) => PropertyValueExpression.CreateValueExpressionParameterSet(parameters),
                        (g, r) => r.ValueExpression = g,
                        ProcessHelper.TryParse
                        ),
                    new InnerProcess<Expression, IExpression, IExpressionParameterSet, PropertyValueExpression>(
                        (g) => PropertyValueExpression.CreateExpressionParameterSet(options.ToInternal().Primitives, parameters, g.Neuron),
                        (g, r) => r.Expression = g,
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                options.ToInternal(),
                (n, r) => r.Neuron = n
            )) != null;

        public IValueExpression ValueExpression { get; private set; }

        public IExpression Expression { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
