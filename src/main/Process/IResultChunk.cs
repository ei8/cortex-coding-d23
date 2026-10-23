namespace ei8.Cortex.Coding.d23.Process
{
    public interface IResultChunk :
        IAggregateChunk
        <
            IEnumerableChunk<INeuronChunk>,
            IConcurrentListChunk<INeuronChunk>
        >
    {
        public IEnumerableChunk<INeuronChunk> DigitValues { get; }
        public IConcurrentListChunk<INeuronChunk> Value { get; }
    }

    public interface IResultChunkStatic<T> :
        IResultChunk
        where T : IResultChunk
    {
        static abstract T CloneWithoutValue(T original);
    }
}
