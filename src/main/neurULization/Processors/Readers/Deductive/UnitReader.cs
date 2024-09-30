using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class UnitReader : IUnitReader
    {
        public IEnumerable<IGrannyQuery> GetQueries(IUnitParameterSet parameters) =>
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

        public bool TryParse(Ensemble ensemble, IUnitParameterSet parameters, out IUnit result)
        {
            result = null;

            var tempResult = new Unit();
            tempResult.Value = parameters.Value;
            tempResult.Type = parameters.Type;

            tempResult.TryParseCore(
                ensemble,
                new[] { tempResult.Value.Id },
                new[] { new LevelParser(new PresynapticByPostsynapticSibling(tempResult.Type.Id)) },
                (n) => tempResult.Neuron = n,
                ref result
                );

            return result != null;
        }
    }
}
