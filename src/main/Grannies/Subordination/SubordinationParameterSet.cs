using neurUL.Common.Domain.Model;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class SubordinationParameterSet : ISubordinationParameterSet
    {
        public SubordinationParameterSet(
            IUnitParameterSet headParameters,
            IEnumerable<IUnitParameterSet> dependentsParameters,
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

        public IUnitParameterSet HeadParameters { get; }

        public IEnumerable<IUnitParameterSet> DependentsParameters { get; }

        public IEnsembleRepository EnsembleRepository { get; }

        public string UserId { get; }
    }
}
