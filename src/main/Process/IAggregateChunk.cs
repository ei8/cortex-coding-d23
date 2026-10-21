using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public interface IAggregateChunk : IChunk
    {
        IEnumerable<IChunk?> Chunks { get; }
    }

    public interface IAggregateChunk<T1> : IChunk
        where T1 : IChunk?
    {
        T1 Content1 { get; }
    }

    public interface IAggregateChunk<T1, T2> : IAggregateChunk<T1>
        where T1 : IChunk?
        where T2 : IChunk?
    {
        T2 Content2 { get; }
    }

    public interface IAggregateChunk<T1, T2, T3> : IAggregateChunk<T1, T2>
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
    {
        T3 Content3 { get; }
    }

    public interface IAggregateChunk<T1, T2, T3, T4> : IAggregateChunk<T1, T2, T3>
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
        where T4 : IChunk?
    {
        T4 Content4 { get; }
    }

    public interface IAggregateChunk<T1, T2, T3, T4, T5> : IAggregateChunk<T1, T2, T3, T4>
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
        where T4 : IChunk?
        where T5 : IChunk?
    {
        T5 Content5 { get; }
    }

    public interface IAggregateChunk<T1, T2, T3, T4, T5, T6> : IAggregateChunk<T1, T2, T3, T4, T5>
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
        where T4 : IChunk?
        where T5 : IChunk?
        where T6 : IChunk?
    {
        T6 Content6 { get; }
    }
}
