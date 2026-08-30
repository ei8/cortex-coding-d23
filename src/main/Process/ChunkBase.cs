namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class ChunkBase<T1>
    (
        T1 content1
    ) :
        IChunk<T1>
    {
        public T1 Content { get; } = content1;
    }

    public abstract class ChunkBase<T1, T2>
    (
        T1 content1,
        T2 content2
    ) :
        ChunkBase<T1>(content1),
        IChunk<T1, T2>
    {
        public T2 Content2 { get; } = content2;
    }

    public abstract class ChunkBase<T1, T2, T3>
    (
        T1 content1,
        T2 content2,
        T3 content3
    ) :
        ChunkBase<T1, T2>(content1, content2),
        IChunk<T1, T2, T3>
    {
        public T3 Content3 { get; } = content3;
    }
}
