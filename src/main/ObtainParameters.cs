using ei8.Cortex.Coding.d23.neurULization;
using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23
{
    public class ObtainParameters
    {
        public ObtainParameters(
            Ensemble ensemble,
            Id23neurULizerOptions options
            )
        {
            AssertionConcern.AssertArgumentNotNull(ensemble, nameof(ensemble));
            AssertionConcern.AssertArgumentNotNull(options, nameof(options));
            
            this.Ensemble = ensemble;
            this.Options = options;
        }

        public Ensemble Ensemble { get; }

        public Id23neurULizerOptions Options { get; } 
    }
}
