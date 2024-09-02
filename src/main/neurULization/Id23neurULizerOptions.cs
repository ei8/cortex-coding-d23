using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface Id23neurULizerOptions
    {
        // TODO: remove so that parameters are fixed and less arbitrary
        // TODO: IEnsembleRepository should not be referenced inside the neurULizer's functions
        IServiceProvider ServiceProvider { get; }

        PrimitiveSet Primitives { get; }

        string UserId { get; }
    }
}
