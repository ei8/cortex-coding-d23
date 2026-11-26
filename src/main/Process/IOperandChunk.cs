namespace ei8.Cortex.Coding.d23.Process
{
    public  interface IOperandChunk :
        IAggregateChunk
        <
            IEnumerableChunk<INeuronChunk>,
            IEnumerableChunk<INeuronChunk>?,
            INeuronChunk?,
            INeuronChunk?
        >,
        IDigitValueArray
    {
        public IEnumerableChunk<INeuronChunk> DigitValues { get; }
        public IEnumerableChunk<INeuronChunk>? Value { get; }
        public INeuronChunk? CurrentDigit { get; }
        public bool TryGetCurrentDigitIndex(out int? index);

        /// <summary>
        /// Updates current digit to an adjacent digit or null if one does not exist.
        /// </summary>
        /// <param name="ascending">A value indicating whether retrieval direction is ascending (lower to higher index) or otherwise.</param>
        public void UpdateTracking(bool ascending = true);
        public void ResetTracking();
    }
}
