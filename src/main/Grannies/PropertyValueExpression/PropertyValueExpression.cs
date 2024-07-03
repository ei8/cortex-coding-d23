using ei8.Cortex.Coding.d23.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies.PropertyValueExpression
{
    public class PropertyValueExpression : IPropertyValueExpression
    {
        public async Task<IPropertyValueExpression> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IPropertyValueExpressionParameterSet parameters)
        {
            var result = new PropertyValueExpression();
            result.ValueExpression = await new ValueExpression().ObtainAsync(
                ensemble,
                primitives,
                PropertyValueExpression.CreatePropertyValueParameterSet(parameters)
                );
            result.Expression = await new Expression().ObtainAsync(
                ensemble,
                primitives,
                PropertyValueExpression.CreateExpressionParameterSet(
                    primitives, 
                    parameters, 
                    result.ValueExpression.Neuron
                    )
            );
            result.Neuron = result.Expression.Neuron;
            return result;
        }

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

        private static IValueExpressionParameterSet CreatePropertyValueParameterSet(IPropertyValueExpressionParameterSet parameters) =>
            new ValueExpressionParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.MatchingNeuronProperty,
                parameters.EnsembleRepository,
                parameters.UserId
            );
        

        public IEnumerable<IGrannyQuery> GetQueries(IPrimitiveSet primitives, IPropertyValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<ValueExpression, IValueExpression, IValueExpressionParameterSet>(
                    (n) => PropertyValueExpression.CreatePropertyValueParameterSet(parameters)
                ),
                new GrannyQueryInner<Expression, IExpression, IExpressionParameterSet>(
                    (n) => PropertyValueExpression.CreateExpressionParameterSet(primitives, parameters, n)
                )
            };

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IPropertyValueExpressionParameterSet parameters, out IPropertyValueExpression result)
        {
            throw new NotImplementedException();
        }

        public IValueExpression ValueExpression { get; private set; }

        public IExpression Expression { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
