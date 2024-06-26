﻿using neurUL.Common.Domain.Model;
using System;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class InstantiatesClassParameterSet : IInstantiatesClassParameterSet
    {
        public InstantiatesClassParameterSet(
            Neuron @class,
            IEnsembleRepository ensembleRepository,
            string userId
            )
        {
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));
            AssertionConcern.AssertArgumentNotNull(ensembleRepository, nameof(ensembleRepository));
            AssertionConcern.AssertArgumentNotEmpty(userId, "Specified value cannot be null or empty.", nameof(userId));

            this.Class = @class;
            this.EnsembleRepository = ensembleRepository; 
            this.UserId = userId;
        }

        public Neuron Class { get; }

        public IEnsembleRepository EnsembleRepository { get; }

        public string UserId { get; }
    }
}
