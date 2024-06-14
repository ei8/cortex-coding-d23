using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Head : IHead
    {
        public async Task<IHead> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IHeadParameterSet parameters)
        {
            var result = new Head();
            result.Value = ensemble.Obtain(parameters.Value);
            result.Neuron = ensemble.Obtain(Neuron.CreateTransient(null, null, null));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Value.Id));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, primitives.Unit.Id));
            return result;
        }

        public IEnumerable<GrannyQuery> GetQueries(IPrimitiveSet primitives, IHeadParameterSet parameters) =>
            new[] {
                new GrannyQuery(
                    new NeuronQuery()
                    {
                        Postsynaptic = new[] {
                            parameters.Value.Id.ToString(),
                            primitives.Unit.Id.ToString()
                        },
                        DirectionValues = DirectionValues.Outbound,
                        Depth = 1
                    }
                )
            };

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IHeadParameterSet parameters, out IHead result)
        {
            result = null;

            var tempResult = new Head();
            tempResult.Value = parameters.Value;

            this.TryParseCore(
                parameters,
                ensemble,
                tempResult,
                new[] { tempResult.Value },
                new[] { new LevelParser(new PresynapticBySibling(primitives.Unit)) },
                (n) => tempResult.Neuron = n,
                ref result
                );

            return result != null;
        }

        public Neuron Value { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
