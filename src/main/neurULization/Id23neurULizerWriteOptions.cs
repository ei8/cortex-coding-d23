namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface Id23neurULizerWriteOptions : IneurULizerWriteOptions, Id23neurULizerOptions
    {
        ei8.Cortex.Coding.d23.neurULization.Processors.Writers.IInstanceProcessor InstanceProcessor { get; }
    }
}
