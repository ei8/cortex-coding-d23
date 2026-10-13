namespace ei8.Cortex.Coding.d23.Process
{
    public interface IResultChunk :
        IChunk
        <
            IEnumerableChunk<INeuronChunk>,
            IListChunk<INeuronChunk>
        >
    {
        public IEnumerableChunk<INeuronChunk> DigitValues { get; }
        public IListChunk<INeuronChunk> Value { get; }
    }

    public interface IResultChunkStatic<T> :
        IResultChunk
        where T : IResultChunk
    {
        static abstract T CloneWithoutValue(T original);
    }
}
