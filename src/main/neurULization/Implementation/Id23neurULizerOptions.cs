using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive;
using ei8.Cortex.Coding.d23.neurULization.Processors.Writers;

namespace ei8.Cortex.Coding.d23.neurULization.Implementation
{
    public interface Id23neurULizerOptions : IneurULizerOptions
    {
        IInstanceWriter InstanceWriter { get; }

        IInstanceReader InductiveInstanceReader { get; }

        IIdInstanceValueWriter IdInstanceValueWriter { get; }

        IInstanceValueReader InductiveInstanceValueReader { get; }

        IMirrorSet Mirrors { get; }
    }
}
