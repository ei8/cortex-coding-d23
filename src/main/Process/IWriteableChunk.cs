namespace ei8.Cortex.Coding.d23.Process
{
    public interface IWriteableChunk<T1> : IReadOnlyChunk<T1>
    {
        new T1 Content { get; set; }
    }

    public interface IWriteableChunk<T1, T2> : IReadOnlyChunk<T1, T2>
    {
        new T2 Content2 { get; set; }
    }

    public interface IWriteableChunk<T1, T2, T3> : IReadOnlyChunk<T1, T2, T3>
    {
        new T3 Content3 { get; set; }
    }
}
