namespace ei8.Cortex.Coding.d23.Process
{
    public interface IWorkingMemory : IChunk
    {
    }

    public interface IWorkingMemory<T1> :
        IWorkingMemory
        where T1 : IChunk
    {
        T1 Chunk1 { get; }
    }

    public interface IWorkingMemory<T1, T2> :
        IWorkingMemory<T1>
        where T1 : IChunk
        where T2 : IChunk
    {
        T2 Chunk2 { get; }
    }

    public interface IWorkingMemory<T1, T2, T3> :
        IWorkingMemory<T1, T2>
        where T1 : IChunk
        where T2 : IChunk
        where T3 : IChunk
    {
        T3 Chunk3 { get; }
    }

    public interface IWorkingMemory<T1, T2, T3, T4> :
        IWorkingMemory<T1, T2, T3>
        where T1 : IChunk
        where T2 : IChunk
        where T3 : IChunk
        where T4 : IChunk
    {
        T4 Chunk4 { get; }
    }

    public interface IWorkingMemory<T1, T2, T3, T4, T5> :
        IWorkingMemory<T1, T2, T3, T4>
        where T1 : IChunk
        where T2 : IChunk
        where T3 : IChunk
        where T4 : IChunk
        where T5 : IChunk
    {
        T5 Chunk5 { get; }
    }
}
