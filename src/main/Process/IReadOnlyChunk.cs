namespace ei8.Cortex.Coding.d23.Process
{
    public interface IReadOnlyChunk<T1> : IChunk
    {
        T1 Content { get; }
    }

    public interface IReadOnlyChunk<T1, T2> : IReadOnlyChunk<T1>
    {
        T2 Content2 { get; }
    }

    public interface IReadOnlyChunk<T1, T2, T3> : IReadOnlyChunk<T1, T2>
    {
        T3 Content3 { get; }
    }
}
