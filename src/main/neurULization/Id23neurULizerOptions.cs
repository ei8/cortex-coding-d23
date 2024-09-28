namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface Id23neurULizerOptions : IneurULizerOptions
    {
        Coding.d23.neurULization.Processors.Writers.IInstanceProcessor WritersInstanceProcessor { get; }

        Coding.d23.neurULization.Processors.Readers.Inductive.IInstanceProcessor ReadersInductiveInstanceProcessor { get; }

        IPrimitiveSet Primitives { get; }
    }
}
