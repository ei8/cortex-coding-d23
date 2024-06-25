using neurUL.Common.Domain.Model;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class ExpressionParameterSet : IExpressionParameterSet
    {
        public ExpressionParameterSet(
            IEnumerable<IUnitParameterSet> dependentsParameters,
            IEnsembleRepository ensembleRepository,
        string userId
            )
        {
            AssertionConcern.AssertArgumentNotNull(dependentsParameters, nameof(dependentsParameters));
            AssertionConcern.AssertArgumentNotNull(ensembleRepository, nameof(ensembleRepository));
            AssertionConcern.AssertArgumentNotEmpty(userId, "Specified value cannot be null or empty.", nameof(userId));

            this.UnitsParameters = dependentsParameters;
            this.EnsembleRepository = ensembleRepository;
            this.UserId = userId;
        }

        public IEnumerable<IUnitParameterSet> UnitsParameters { get; }

        public IEnsembleRepository EnsembleRepository { get; }

        public string UserId { get; }
    }
}
