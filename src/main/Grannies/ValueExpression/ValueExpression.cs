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
            result.ValueInstantiation = await new Value().ObtainAsync(
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
            //result.Value = await new Head().ObtainAsync()

            return result;
        }

        public IEnumerable<GrannyQuery> GetQueries(IPrimitiveSet primitives, IValueExpressionParameterSet parameters)
        {
            throw new NotImplementedException();
        }

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IValueExpressionParameterSet parameters, out IValueExpression result)
        {
            throw new NotImplementedException();
        }

        public IValue ValueInstantiation { get; private set; }

        public IUnit Value { get; private set; }

        public IUnit Of { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
