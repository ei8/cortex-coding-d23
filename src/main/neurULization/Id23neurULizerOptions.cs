using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface Id23neurULizerOptions : IneurULizerOptions
    {
        IServiceProvider ServiceProvider { get; }

        PrimitiveSet Primitives { get; }

        string UserId { get; }
    }
}
