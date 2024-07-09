using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface Id23neurULizationOptions : IneurULizationOptions
    {
        PrimitiveSet Primitives { get; }

        IEnsembleRepository EnsembleRepository { get; }

        string UserId { get; }
    }
}
