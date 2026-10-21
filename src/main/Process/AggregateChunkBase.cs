using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class AggregateChunkBase<T1>
    (
        T1 content1
    ) :
        IAggregateChunk<T1>
        where T1 : IChunk?
    {
        public virtual IEnumerable<IChunk?> Chunks => [this.Content1];

        public T1 Content1 { get; protected set; } = content1;
    }

    public abstract class AggregateChunkBase
    <
        T1, 
        T2
    >
    (
        T1 content1,
        T2 content2
    ) :
        AggregateChunkBase<T1>(content1),
        IAggregateChunk
        <
            T1, 
            T2
        >
        where T1 : IChunk?
        where T2 : IChunk?
    {
        public override IEnumerable<IChunk?> Chunks => [..base.Chunks, this.Content2];

        public T2 Content2 { get; protected set; } = content2;
    }

    public abstract class AggregateChunkBase
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
            T2
        >
        (
            content1, 
            content2
        ),
        IAggregateChunk
        <
            T1, 
            T2, 
            T3
        >
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
    {
        public override IEnumerable<IChunk?> Chunks => [.. base.Chunks, this.Content3];

        public T3 Content3 { get; protected set; } = content3;
    }

    public abstract class AggregateChunkBase
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
            T3
        >
        (
            content1, 
            content2, 
            content3
        ),
        IAggregateChunk
        <
            T1, 
            T2, 
            T3, 
            T4
        >
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
        where T4 : IChunk?
    {
        public override IEnumerable<IChunk?> Chunks => [.. base.Chunks, this.Content4];

        public T4 Content4 { get; protected set; } = content4;
    }

    public abstract class AggregateChunkBase
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
            T4
        >
        (
            content1, 
            content2, 
            content3, 
            content4
        ),
        IAggregateChunk
        <
            T1, 
            T2, 
            T3, 
            T4, 
            T5
        >
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
        where T4 : IChunk?
        where T5 : IChunk?
    {
        public override IEnumerable<IChunk?> Chunks => [.. base.Chunks, this.Content5];

        public T5 Content5 { get; protected set; } = content5;
    }

    public abstract class AggregateChunkBase
    <
        T1, 
        T2, 
        T3, 
        T4, 
        T5, 
        T6
    >
    (
        T1 content1,
        T2 content2,
        T3 content3,
        T4 content4,
        T5 content5,
        T6 content6
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
        IAggregateChunk
        <
            T1, 
            T2, 
            T3, 
            T4, 
            T5, 
            T6
        >
        where T1 : IChunk?
        where T2 : IChunk?
        where T3 : IChunk?
        where T4 : IChunk?
        where T5 : IChunk?
        where T6 : IChunk?
    {
        public override IEnumerable<IChunk?> Chunks => [.. base.Chunks, this.Content6];

        public T6 Content6 { get; protected set; } = content6;
    }
}
