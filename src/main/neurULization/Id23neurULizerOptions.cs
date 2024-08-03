using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface Id23neurULizerOptions
    {
        IServiceProvider ServiceProvider { get; }

        PrimitiveSet Primitives { get; }

        string UserId { get; }
    }
}
