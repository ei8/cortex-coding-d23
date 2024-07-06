using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class ValueParameterSet : IValueParameterSet
    {
        public ValueParameterSet(
            Neuron value,
            Neuron @class,
            ValueMatchByValue valueMatchBy,
            IEnsembleRepository ensembleRepository,
            string userId
            )
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            AssertionConcern.AssertArgumentNotNull(@class, nameof(@class));
            AssertionConcern.AssertArgumentNotNull(ensembleRepository, nameof(ensembleRepository));
            AssertionConcern.AssertArgumentNotEmpty(userId, "Specified value cannot be null or empty.", nameof(userId));

            this.Value = value;
            this.Class = @class;
            this.ValueMatchBy = valueMatchBy;
            this.EnsembleRepository = ensembleRepository;
            this.UserId = userId;
        }

        public Neuron Value { get; }

        public Neuron Class { get; }

        public ValueMatchByValue ValueMatchBy { get; }

        public IEnsembleRepository EnsembleRepository { get; }

        public string UserId { get; }
    }
}
