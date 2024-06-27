using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class ValueExpression : IValueExpression
    {
        public async Task<IValueExpression> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IValueExpressionParameterSet parameters)
        {
            var result = new ValueExpression();
            result.ValueInstance = await new Value().ObtainAsync(
                ensemble,
                primitives,
                new ValueParameterSet(
                    parameters.Value,
                    parameters.Class,
                    parameters.MatchingNeuronProperty,
                    parameters.EnsembleRepository,
                    parameters.UserId
                    ),
                parameters.EnsembleRepository,
                parameters.UserId
                );
            result.Expression = await new Expression().ObtainAsync(
                ensemble,
                primitives,
                new ExpressionParameterSet(
                    new[]
                    {
                        new UnitParameterSet(
                            result.ValueInstance.Neuron,
                            primitives.Unit
                        )
                    },
                    parameters.EnsembleRepository, 
                    parameters.UserId
                ),
                parameters.EnsembleRepository,
                parameters.UserId
            );
            result.Neuron = result.Expression.Neuron;
            return result;
        }

        public IEnumerable<IGrannyQuery> GetQueries(IPrimitiveSet primitives, IValueExpressionParameterSet parameters)
        {
            throw new NotImplementedException();
        }

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IValueExpressionParameterSet parameters, out IValueExpression result)
        {
            throw new NotImplementedException();
        }

        public IValue ValueInstance { get; private set; }

        public IExpression Expression { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
