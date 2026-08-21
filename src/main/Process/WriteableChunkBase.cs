namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class WriteableChunkBase<T1>
    (
        T1 content1
    ) : 
        IWriteableChunk<T1>
    {
        public T1 Content { get; set; } = content1;
    }

    public abstract class WriteableChunkBase<T1, T2>
    (
        T1 content1,
        T2 content2
    ) :
        WriteableChunkBase<T1>(content1),
        IWriteableChunk<T1, T2>
    {
        public T2 Content2 { get; set; } = content2;
    }

    public abstract class WriteableChunkBase<T1, T2, T3>
    (
        T1 content1,
        T2 content2,
        T3 content3
    ) :
        WriteableChunkBase<T1, T2>(content1, content2),
        IWriteableChunk<T1, T2>
    {
        public T3 Content3 { get; set; } = content3;
    }
}
