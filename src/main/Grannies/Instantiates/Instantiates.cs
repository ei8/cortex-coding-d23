using ei8.Cortex.Coding.d23.Filters;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Instantiates : IInstantiates
    {
        public Instantiates()
        {
        }

        public async Task<Neuron> BuildAsync(Ensemble ensemble, ICoreSet coreSet, IInstantiatesParameterSet parameterSet)
        {
            var subordination = ensemble.Obtain(coreSet.Subordination);
            var instantiatesUnit = ensemble.Obtain(coreSet.InstantiatesUnit);
            var classDirectObject = await parameterSet.Dependency.ObtainAsync(
                ensemble,
                coreSet,
                new DependencyParameterSet(
                    parameterSet.Class,
                    coreSet.DirectObject
                ),
                parameterSet.NeuronRepository,
                parameterSet.UserId
                );
            var instantiatesClass = ensemble.Obtain(Neuron.CreateTransient(null, null, null));
            ensemble.AddReplace(Terminal.CreateTransient(instantiatesClass.Id, subordination.Id));
            ensemble.AddReplace(Terminal.CreateTransient(instantiatesClass.Id, instantiatesUnit.Id));
            ensemble.AddReplace(Terminal.CreateTransient(instantiatesClass.Id, classDirectObject.Id));

            return instantiatesClass;
        }

        public IEnumerable<Library.Common.NeuronQuery> GetQueries(ICoreSet coreSet, IInstantiatesParameterSet parameterSet) =>
            new[] {
                new NeuronQuery()
                {
                    Id = new[] { parameterSet.Class.Id.ToString() },
                    DirectionValues = DirectionValues.Any,
                    Depth = 3,
                    TraversalMinimumDepthPostsynaptic = new[] {
                        new DepthIdsPair {
                            Depth = 3,
                            Ids = new[] {
                                coreSet.Subordination.Id,
                                coreSet.InstantiatesUnit.Id
                            }
                        }
                    }
                }
            };

        public bool TryParse(Ensemble ensemble, ICoreSet coreSet, IInstantiatesParameterSet parameterSet, out Neuron result)
        {
            result = null;
            IEnumerable<Neuron> neurons = new[] { parameterSet.Class };

            var levelParsers = new LevelParser[]
            {
                new LevelParser(new PresynapticBySibling(coreSet.DirectObject.Id)),
                new LevelParser(new PresynapticBySibling(
                    coreSet.Subordination.Id,
                    coreSet.InstantiatesUnit.Id
                    ))
            };

            foreach (var levelParser in levelParsers)
                neurons = levelParser.Evaluate(ensemble, neurons);

            if (neurons.Count() == 1)
                result = neurons.Single();

            return result != null;
        }
    }
}
