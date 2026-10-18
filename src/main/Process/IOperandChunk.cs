namespace ei8.Cortex.Coding.d23.Process
{
    public  interface IOperandChunk :
        IAggregateChunk
        <
            IEnumerableChunk<INeuronChunk>,
            IEnumerableChunk<INeuronChunk>?,
            INeuronChunk?,
            INeuronChunk?
        >
    {
        public IEnumerableChunk<INeuronChunk> DigitValues { get; }
        public IEnumerableChunk<INeuronChunk>? Value { get; }
        public INeuronChunk? PreviousDigit { get; }
        public INeuronChunk? CurrentDigit { get; }
        public void UpdateTracking(bool next = true);
    }
}
