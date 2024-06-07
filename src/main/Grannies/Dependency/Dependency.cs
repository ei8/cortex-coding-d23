using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Dependency : IDependency
    {
        public Dependency()
        {
        }

        public async Task<IDependency> BuildAsync(Ensemble ensemble, ICoreSet coreSet, IDependencyParameterSet parameterSet)
        {
            var result = new Dependency();
            result.Value = ensemble.Obtain(parameterSet.Value);
            result.Type = ensemble.Obtain(parameterSet.Type);
            result.Neuron = ensemble.Obtain(Neuron.CreateTransient(null, null, null));
            // add dependency to ensemble
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Value.Id));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Type.Id));
            return result;
        }

        public IEnumerable<Library.Common.NeuronQuery> GetQueries(ICoreSet coreSet, IDependencyParameterSet parameterSet) =>
            new[] {
                new NeuronQuery()
                {
                    Postsynaptic = new[] { 
                        parameterSet.Value.Id.ToString(),
                        parameterSet.Type.ToString()
                    },
                    DirectionValues = DirectionValues.Outbound,
                    Depth = 1
                }
            };

        public bool TryParse(Ensemble ensemble, ICoreSet coreSet, IDependencyParameterSet parameterSet, out IDependency result)
        {
            result = null;

            var tempResult = new Dependency();
            tempResult.Value = parameterSet.Value;
            tempResult.Type = parameterSet.Type;
            
            this.TryParseCore(
                parameterSet,
                ensemble,
                tempResult,
                new[] { tempResult.Value },
                new[] { new LevelParser(new PresynapticBySibling(tempResult.Type)) },
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
