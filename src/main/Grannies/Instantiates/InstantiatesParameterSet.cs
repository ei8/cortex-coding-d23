using neurUL.Common.Domain.Model;
using System;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstantiatesParameterSet : IInstantiatesParameterSet
    {
        public InstantiatesParameterSet(
            Neuron @class,
            IDependency dependency,
            IEnsembleRepository neuronRepository,
            string userId
            )
        {
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));
            AssertionConcern.AssertArgumentNotNull(dependency, nameof(dependency));
            AssertionConcern.AssertArgumentNotNull(neuronRepository, nameof(neuronRepository));
            AssertionConcern.AssertArgumentNotEmpty(userId, "Specified value cannot be null or empty.", nameof(userId));

            this.Class = @class;
            this.Dependency = dependency;
            this.NeuronRepository = neuronRepository; 
            this.UserId = userId;
        }

        public Neuron Class { get; }

        public IDependency Dependency { get; }

        public IEnsembleRepository NeuronRepository { get; }

        public string UserId { get; }
    }
}
