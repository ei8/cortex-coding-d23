using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Head : IHead
    {
        public async Task<IHead> BuildAsync(Ensemble ensemble, ICoreSet coreSet, IHeadParameterSet parameterSet)
        {
            var result = new Head();
            result.Value = ensemble.Obtain(parameterSet.Value);
            result.Neuron = ensemble.Obtain(Neuron.CreateTransient(null, null, null));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Value.Id));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, coreSet.Unit.Id));
            return result;
        }

        public IEnumerable<NeuronQuery> GetQueries(ICoreSet coreSet, IHeadParameterSet parameterSet) =>
            new[] {
                new NeuronQuery()
                {
                    Postsynaptic = new[] {
                        parameterSet.Value.Id.ToString(),
                        coreSet.Unit.Id.ToString()
                    },
                    DirectionValues = DirectionValues.Outbound,
                    Depth = 1
                }
            };

        public bool TryParse(Ensemble ensemble, ICoreSet coreSet, IHeadParameterSet parameterSet, out IHead result)
        {
            result = null;

            var tempResult = new Head();
            tempResult.Value = parameterSet.Value;

            this.TryParseCore(
                parameterSet,
                ensemble,
                tempResult,
                new[] { tempResult.Value },
                new[] { new LevelParser(new PresynapticBySibling(coreSet.Unit)) },
                (n) => tempResult.Neuron = n,
                ref result
                );

            return result != null;
        }

        public Neuron Value { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
