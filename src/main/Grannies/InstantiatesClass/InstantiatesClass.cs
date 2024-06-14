using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstantiatesClass : IInstantiatesClass
    {
        public async Task<IInstantiatesClass> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters)
        {
            var result = new InstantiatesClass();

            var subordination = await new Subordination().BuildAsync(
                ensemble,
                primitives,
                InstantiatesClass.CreateSubordinationParameterSet(primitives, parameters)
                );

            result.Class = subordination.Dependents.Single();
            result.Neuron = subordination.Neuron;

            return result;
        }

        public IEnumerable<GrannyQuery> GetQueries(IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters) =>
            new Subordination().GetQueries(
                primitives,
                InstantiatesClass.CreateSubordinationParameterSet(primitives, parameters)
                );

        private static SubordinationParameterSet CreateSubordinationParameterSet(IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters)
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
                parameters.EnsembleRepository,
                parameters.UserId
            );
        }

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result)
        {
            result = null;

            var tempResult = new InstantiatesClass();

            if (new Subordination().TryParse(
                ensemble,
                primitives,
                InstantiatesClass.CreateSubordinationParameterSet(primitives, parameters),
                out ISubordination subordination
                )
                )
            {
                tempResult.Class = subordination.Dependents.Single();
                tempResult.Neuron = subordination.Neuron;
                result = tempResult;
            }

            return result != null;
        }

        public IDependent Class { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
