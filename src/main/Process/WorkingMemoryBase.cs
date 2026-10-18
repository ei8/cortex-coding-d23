namespace ei8.Cortex.Coding.d23.Process
{
    public class WorkingMemoryBase
    <
        T1
    >
    (
        T1 content1
    ): 
        AggregateChunkBase<T1>
        (
            content1
        ),
        IWorkingMemory
        where T1 : IChunk?
    {
    }

    public class WorkingMemoryBase
    <
        T1,
        T2
    >
    (
        T1 content1,
        T2 content2
    ) :
        AggregateChunkBase
        <
            T1, 
            T2
        >
        (
            content1,
            content2
        ),
        IWorkingMemory
        where T1 : IChunk?
        where T2 : IChunk?
    {
    }

    public class WorkingMemoryBase
    <
        T1,
        T2,
        T3
    >
    (
        T1 content1,
        T2 content2,
        T3 content3
    ) :
        AggregateChunkBase
        <
            T1,
            T2,
            T3
        >
        (
            content1,
            content2,
            content3
        ),
        IWorkingMemory
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
    {
    }

    public class WorkingMemoryBase
    <
        T1,
        T2,
        T3,
        T4
    >
    (
        T1 content1,
        T2 content2,
        T3 content3,
        T4 content4
    ) :
        AggregateChunkBase
        <
            T1,
            T2,
            T3,
            T4
        >
        (
            content1,
            content2,
            content3,
            content4
        ),
        IWorkingMemory
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
        where T4 : IChunk?
    {
    }

    public class WorkingMemoryBase
    <
        T1,
        T2,
        T3,
        T4,
        T5
    >
    (
        T1 content1,
        T2 content2,
        T3 content3,
        T4 content4,
        T5 content5
    ) :
        AggregateChunkBase
        <
            T1,
            T2,
            T3,
            T4,
            T5
        >
        (
            content1,
            content2,
            content3,
            content4,
            content5
        ),
        IWorkingMemory
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
        where T4 : IChunk?
        where T5 : IChunk?
    {
    }
}
