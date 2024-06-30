using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class ValueExpression : IValueExpression
    {
        public async Task<IValueExpression> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IValueExpressionParameterSet parameters)
        {
            var result = new ValueExpression();
            result.Value = await new Value().ObtainAsync(
                ensemble,
                primitives,
                ValueExpression.CreateValueParameterSet(parameters)
                );
            result.Expression = await new Expression().ObtainAsync(
                ensemble,
                primitives,
                new ExpressionParameterSet(
                    new[]
                    {
                        new UnitParameterSet(
                            result.Value.Neuron,
                            primitives.Unit
                        )
                    },
                    parameters.EnsembleRepository,
                    parameters.UserId
                )
            );
            result.Neuron = result.Expression.Neuron;
            return result;
        }

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

        private static ExpressionParameterSet CreateExpressionParameterSet(IPrimitiveSet primitives, IValueExpressionParameterSet parameters, Neuron n)
        {
            return new ExpressionParameterSet(new[]
                    {
                        new UnitParameterSet(
                            n,
                            primitives.Unit
                        ),
                    },
                    parameters.EnsembleRepository,
                    parameters.UserId
                );
        }

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IValueExpressionParameterSet parameters, out IValueExpression result)
        {
            result = null;

            var tempResult = new ValueExpression();

            if (new Value().TryParse(
                ensemble,
                primitives,
                ValueExpression.CreateValueParameterSet(parameters),
                out IValue v
                ))
            {
                tempResult.Value = v;

                if (new Expression().TryParse(
                    ensemble,
                    primitives,
                    ValueExpression.CreateExpressionParameterSet(primitives, parameters, tempResult.Value.Neuron),
                    out IExpression e
                    ))
                {
                    tempResult.Expression = e;
                    tempResult.Neuron = e.Neuron;
                    result = tempResult;
                }
            }

            return result != null;
        }

        public IValue Value { get; private set; }

        public IExpression Expression { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
