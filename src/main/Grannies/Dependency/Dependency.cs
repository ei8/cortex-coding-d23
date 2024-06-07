using ei8.Cortex.Coding.d23.Filters;
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
        
        public async Task<Neuron> BuildAsync(Ensemble ensemble, ICoreSet coreSet, IDependencyParameterSet parameterSet)
        {
            Neuron value = ensemble.Obtain(parameterSet.Value);
            Neuron dependencyType = ensemble.Obtain(parameterSet.Type);
            Neuron dependency = ensemble.Obtain(Neuron.CreateTransient(null, null, null));
            // add dependency to ensemble
            ensemble.AddReplace(Terminal.CreateTransient(dependency.Id, value.Id));
            ensemble.AddReplace(Terminal.CreateTransient(dependency.Id, dependencyType.Id));
            return dependency;
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

        public bool TryParse(Ensemble ensemble, ICoreSet coreSet, IDependencyParameterSet parameterSet, out Neuron result)
        {
            result = null;
            IEnumerable<Neuron> neurons = new[] { parameterSet.Value };

            var levelParsers = new LevelParser[]
            {
                new LevelParser(new PresynapticBySibling(parameterSet.Type.Id)),
            };

            foreach (var levelParser in levelParsers)
                neurons = levelParser.Evaluate(ensemble, neurons);

            if (neurons.Count() == 1)
                result = neurons.Single();

            return result != null;

        }
    }
}
