using ei8.Cortex.Coding.d23.Queries;
using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Unit : IUnit
    {
        public async Task<IUnit> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IUnitParameterSet parameters)
        {
            var result = new Unit();
            result.Value = ensemble.Obtain(parameters.Value);
            result.Type = ensemble.Obtain(parameters.Type);
            result.Neuron = ensemble.Obtain(Neuron.CreateTransient(null, null, null));
            // add dependency to ensemble
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Value.Id));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Type.Id));
            return result;
        }

        public IEnumerable<IGrannyQuery> GetQueries(IPrimitiveSet primitives, IUnitParameterSet parameters) =>
            new[] {
                new GrannyQuery(
                    new NeuronQuery()
                    {
                        Postsynaptic = new[] {
                            parameters.Value.Id.ToString(),
                            parameters.Type.Id.ToString()
                        },
                        DirectionValues = DirectionValues.Outbound,
                        Depth = 1
                    }
                )
            };

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IUnitParameterSet parameters, out IUnit result)
        {
            result = null;

            var tempResult = new Unit();
            tempResult.Value = parameters.Value;
            tempResult.Type = parameters.Type;

            this.TryParseCore(
                parameters,
                ensemble,
                tempResult,
                new[] { tempResult.Value.Id },
                new[] { new LevelParser(new PresynapticBySibling(tempResult.Type.Id)) },
                (n) => tempResult.Neuron = n,
                ref result
                );

            return result != null;
        }

        public Neuron Value { get; private set; }

        public Neuron Type { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
