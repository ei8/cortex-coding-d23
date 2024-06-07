using ei8.Cortex.Coding.d23.Selectors;
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

        public async Task<IInstantiates> BuildAsync(Ensemble ensemble, ICoreSet coreSet, IInstantiatesParameterSet parameterSet)
        {
            var result = new Instantiates();
            result.Subordination = ensemble.Obtain(coreSet.Subordination);
            result.InstantiatesUnit = ensemble.Obtain(coreSet.InstantiatesUnit);
            result.ClassDirectObject = await parameterSet.Dependency.ObtainAsync(
                ensemble,
                coreSet,
                new DependencyParameterSet(
                    parameterSet.Class,
                    coreSet.DirectObject
                ),
                parameterSet.NeuronRepository,
                parameterSet.UserId
                );
            result.Neuron = ensemble.Obtain(Neuron.CreateTransient(null, null, null));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Subordination.Id));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.InstantiatesUnit.Id));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.ClassDirectObject.Neuron.Id));

            return result;
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

        public bool TryParse(Ensemble ensemble, ICoreSet coreSet, IInstantiatesParameterSet parameterSet, out IInstantiates result)
        {
            result = null;

            var tempResult = new Instantiates();
            tempResult.Subordination = coreSet.Subordination;
            tempResult.InstantiatesUnit = coreSet.InstantiatesUnit;
            if ((new Dependency()).TryParse(
                ensemble,
                coreSet,
                new DependencyParameterSet(
                    parameterSet.Class,
                    coreSet.DirectObject
                    ),
                out IDependency classDirectObject
                ))
            {
                tempResult.ClassDirectObject = classDirectObject;

                this.TryParseCore(
                    parameterSet,
                    ensemble, 
                    tempResult,
                    new[] { tempResult.ClassDirectObject.Neuron },
                    new []
                    {
                        new LevelParser(new PresynapticBySibling(
                            tempResult.Subordination,
                            tempResult.InstantiatesUnit
                            ))
                    }, 
                    (n) => tempResult.Neuron = n,
                    ref result
                    );
            }

            return result != null;
        }

        public Neuron Subordination { get; private set; }

        public Neuron InstantiatesUnit { get; private set; }

        public IDependency ClassDirectObject { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
