namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class WorkingMemoryBase : 
        IWorkingMemory
    {
    }

    public abstract class WorkingMemoryBase<T>(T chunk1) : 
        IWorkingMemory<T>
        where T : IChunk
    {
        public T Chunk1 { get; } = chunk1;
    }

    public abstract class WorkingMemoryBase<T1, T2>
    (
        T1 chunk1,
        T2 chunk2
    ) : 
        WorkingMemoryBase<T1>
        (
            chunk1
        )
        where T1 : IChunk
        where T2 : IChunk
    {
        public T2 Chunk2 { get; } = chunk2;
    }

    public abstract class WorkingMemoryBase<T1, T2, T3>
    (
        T1 chunk1,
        T2 chunk2,
        T3 chunk3
    ) :
        WorkingMemoryBase<T1, T2>
        (
            chunk1,
            chunk2
        )
        where T1 : IChunk
        where T2 : IChunk
        where T3 : IChunk
    {
        public T3 Chunk3 { get; } = chunk3;
    }

    public abstract class WorkingMemoryBase<T1, T2, T3, T4>
    (
        T1 chunk1,
        T2 chunk2,
        T3 chunk3,
        T4 chunk4
    ) :
        WorkingMemoryBase<T1, T2, T3>
        (
            chunk1,
            chunk2,
            chunk3
        )
        where T1 : IChunk
        where T2 : IChunk
        where T3 : IChunk
        where T4 : IChunk
    {
        public T4 Chunk4 { get; } = chunk4;
    }

    public abstract class WorkingMemoryBase<T1, T2, T3, T4, T5>
    (
        T1 chunk1,
        T2 chunk2,
        T3 chunk3,
        T4 chunk4,
        T5 chunk5
    ) :
        WorkingMemoryBase<T1, T2, T3, T4>
        (
            chunk1,
            chunk2,
            chunk3,
            chunk4
        )
        where T1 : IChunk
        where T2 : IChunk
        where T3 : IChunk
        where T4 : IChunk
        where T5 : IChunk
    {
        public T5 Chunk5 { get; } = chunk5;
    }

    public abstract class WorkingMemoryBase<T1, T2, T3, T4, T5, T6>
    (
        T1 chunk1,
        T2 chunk2,
        T3 chunk3,
        T4 chunk4,
        T5 chunk5,
        T6 chunk6
    ) :
        WorkingMemoryBase<T1, T2, T3, T4, T5>
        (
            chunk1,
            chunk2,
            chunk3,
            chunk4,
            chunk5
        )
        where T1 : IChunk
        where T2 : IChunk
        where T3 : IChunk
        where T4 : IChunk
        where T5 : IChunk
        where T6 : IChunk
    {
        public T6 Chunk6 { get; } = chunk6;
    }

    public abstract class WorkingMemoryBase<T1, T2, T3, T4, T5, T6, T7>
    (
        T1 chunk1,
        T2 chunk2,
        T3 chunk3,
        T4 chunk4,
        T5 chunk5,
        T6 chunk6,
        T7 chunk7
    ) :
        WorkingMemoryBase<T1, T2, T3, T4, T5, T6>
        (
            chunk1,
            chunk2,
            chunk3,
            chunk4,
            chunk5,
            chunk6
        )
        where T1 : IChunk
        where T2 : IChunk
        where T3 : IChunk
        where T4 : IChunk
        where T5 : IChunk
        where T6 : IChunk
        where T7 : IChunk
    {
        public T7 Chunk7 { get; } = chunk7;
    }
}
