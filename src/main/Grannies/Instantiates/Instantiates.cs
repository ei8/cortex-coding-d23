using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Instantiates : IInstantiates
    {
        public async Task<IInstantiates> BuildAsync(Ensemble ensemble, ICoreSet coreSet, IInstantiatesParameterSet parameterSet)
        {
            var result = new Instantiates();
            var subordination = ensemble.Obtain(coreSet.Subordination);
            var instantiatesUnit = ensemble.Obtain(coreSet.InstantiatesUnit);
            result.ClassDirectObject = await new Dependent().ObtainAsync(
                ensemble,
                coreSet,
                new DependentParameterSet(
                    parameterSet.Class,
                    coreSet.DirectObject
                ),
                parameterSet.NeuronRepository,
                parameterSet.UserId
                );
            result.Neuron = ensemble.Obtain(Neuron.CreateTransient(null, null, null));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, subordination.Id));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, instantiatesUnit.Id));
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
                    // less than 3 edges away otherwise should have postsynaptic of subordination or instantiates unit 
                    // TODO: or should this be renamed to TraversalMaximumDepthPostsynaptic
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
            if (new Dependent().TryParse(
                ensemble,
                coreSet,
                new DependentParameterSet(
                    parameterSet.Class,
                    coreSet.DirectObject
                    ),
                out IDependent classDirectObject
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
                            coreSet.Subordination,
                            coreSet.InstantiatesUnit
                            ))
                    }, 
                    (n) => tempResult.Neuron = n,
                    ref result
                    );
            }

            return result != null;
        }

        public IDependent ClassDirectObject { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
