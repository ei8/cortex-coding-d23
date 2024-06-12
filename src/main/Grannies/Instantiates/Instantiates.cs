using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Instantiates : IInstantiates
    {
        public async Task<IInstantiates> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IInstantiatesParameterSet parameters)
        {
            var result = new Instantiates();

            var subordination = await new Subordination().BuildAsync(
                ensemble,
                primitives,
                Instantiates.CreateSubordinationParameterSet(primitives, parameters)
                );

            result.ClassDirectObject = subordination.Dependents.Single();
            result.Neuron = subordination.Neuron;

            return result;
        }

        public IEnumerable<NeuronQuery> GetQueries(IPrimitiveSet primitives, IInstantiatesParameterSet parameters) =>
            new Subordination().GetQueries(
                primitives,
                Instantiates.CreateSubordinationParameterSet(primitives, parameters)
                );

        private static SubordinationParameterSet CreateSubordinationParameterSet(IPrimitiveSet primitives, IInstantiatesParameterSet parameters)
        {
            return new SubordinationParameterSet(
                new HeadParameterSet(primitives.Instantiates),
                new IDependentParameterSet[]
                {
                    new DependentParameterSet(
                        parameters.Class,
                        primitives.DirectObject
                        )
                },
                parameters.NeuronRepository,
                parameters.UserId
            );
        }

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IInstantiatesParameterSet parameters, out IInstantiates result)
        {
            result = null;

            var tempResult = new Instantiates();

            if (new Subordination().TryParse(
                ensemble,
                primitives,
                Instantiates.CreateSubordinationParameterSet(primitives, parameters),
                out ISubordination subordination
                )
                )
            {
                tempResult.ClassDirectObject = subordination.Dependents.Single();
                tempResult.Neuron = subordination.Neuron;
                result = tempResult;
            }

            return result != null;
        }

        public IDependent ClassDirectObject { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
