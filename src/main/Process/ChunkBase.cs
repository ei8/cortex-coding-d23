namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class ChunkBase<T1>
    (
        T1 content1
    ) :
        IChunk<T1>
        where T1 : IChunk?
    {
        public T1 Content1 { get; protected set; } = content1;
    }

    public abstract class ChunkBase<T1, T2>
    (
        T1 content1,
        T2 content2
    ) :
        ChunkBase<T1>(content1),
        IChunk<T1, T2>
        where T1 : IChunk?
        where T2 : IChunk?
    {
        public T2 Content2 { get; protected set; } = content2;
    }

    public abstract class ChunkBase<T1, T2, T3>
    (
        T1 content1,
        T2 content2,
        T3 content3
    ) :
        ChunkBase<T1, T2>(content1, content2),
        IChunk<T1, T2, T3>
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
    {
        public T3 Content3 { get; protected set; } = content3;
    }

    public abstract class ChunkBase<T1, T2, T3, T4>
    (
        T1 content1,
        T2 content2,
        T3 content3,
        T4 content4
    ) :
        ChunkBase<T1, T2, T3>(content1, content2, content3),
        IChunk<T1, T2, T3, T4>
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
        where T4 : IChunk?
    {
        public T4 Content4 { get; protected set; } = content4;
    }

    public abstract class ChunkBase<T1, T2, T3, T4, T5>
    (
        T1 content1,
        T2 content2,
        T3 content3,
        T4 content4,
        T5 content5
    ) :
        ChunkBase<T1, T2, T3, T4>(content1, content2, content3, content4),
        IChunk<T1, T2, T3, T4, T5>
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
        where T4 : IChunk?
        where T5 : IChunk?
    {
        public T5 Content5 { get; protected set; } = content5;
    }
}
