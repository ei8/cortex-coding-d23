using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23
{
    public class ObtainParameters
    {
        public ObtainParameters(
            Ensemble ensemble,
            IPrimitiveSet primitives,
            IEnsembleRepository ensembleRepository,
            string userId
            )
        {
            AssertionConcern.AssertArgumentNotNull(ensemble, nameof(ensemble));
            AssertionConcern.AssertArgumentNotNull(primitives, nameof(primitives));
            AssertionConcern.AssertArgumentNotNull(ensembleRepository, nameof(ensembleRepository));
            AssertionConcern.AssertArgumentNotEmpty(userId, "UserId cannot be null or empty.", nameof(userId));

            this.Ensemble = ensemble;
            this.Primitives = primitives;
            this.EnsembleRepository = ensembleRepository;
            this.UserId = userId;
        }

        public Ensemble Ensemble { get; }
        public IPrimitiveSet Primitives { get; }
        public IEnsembleRepository EnsembleRepository { get; }
        public string UserId { get; }
    }
}
