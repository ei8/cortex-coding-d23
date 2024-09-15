using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface Id23neurULizerWriteOptions : IneurULizerWriteOptions, Id23neurULizerOptions
    {
        Processors.Writers.IInstanceProcessor InstanceProcessor { get; }

        IDictionary<string, Ensemble> Cache { get; }
    }
}
