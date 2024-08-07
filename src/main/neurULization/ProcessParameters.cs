using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class ProcessParameters
    {
        public ProcessParameters(
            Ensemble ensemble,
            Id23neurULizerOptions options
            )
        {
            AssertionConcern.AssertArgumentNotNull(ensemble, nameof(ensemble));
            AssertionConcern.AssertArgumentNotNull(options, nameof(options));

            Ensemble = ensemble;
            Options = options;
        }

        public Ensemble Ensemble { get; }

        public Id23neurULizerOptions Options { get; }
    }
}
