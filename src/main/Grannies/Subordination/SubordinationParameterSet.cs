using neurUL.Common.Domain.Model;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class SubordinationParameterSet : ISubordinationParameterSet
    {
        public SubordinationParameterSet(
            IHeadParameterSet headParameters,
            IEnumerable<IDependentParameterSet> dependentsParameters,
            IEnsembleRepository ensembleRepository,
        string userId
            )
        {
            AssertionConcern.AssertArgumentNotNull(headParameters, nameof(headParameters));
            AssertionConcern.AssertArgumentNotNull(dependentsParameters, nameof(dependentsParameters));
            AssertionConcern.AssertArgumentNotNull(ensembleRepository, nameof(ensembleRepository));
            AssertionConcern.AssertArgumentNotEmpty(userId, "Specified value cannot be null or empty.", nameof(userId));

            this.HeadParameters = headParameters;
            this.DependentsParameters = dependentsParameters;
            this.EnsembleRepository = ensembleRepository;
            this.UserId = userId;
        }

        public IHeadParameterSet HeadParameters { get; }

        public IEnumerable<IDependentParameterSet> DependentsParameters { get; }

        public IEnsembleRepository EnsembleRepository { get; }

        public string UserId { get; }
    }
}
