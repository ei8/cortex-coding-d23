using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Process
{
    public class ResultChunk
    (
        IEnumerableChunk<INeuronChunk> digitValues,
        IConcurrentListChunk<INeuronChunk> value,
        [CallerArgumentExpression(nameof(value))] string name = ""
    ) :
        AggregateChunkBase
        <
            IEnumerableChunk<INeuronChunk>,
            IConcurrentListChunk<INeuronChunk>
        >
        (
            digitValues,
            value
        ),
        IResultChunk,
        INamedChunk,
        IResultChunkStatic<ResultChunk>
    {
        private INeuronChunk[]? digitValuesArray;

        public ResultChunk(IEnumerableChunk<INeuronChunk> digitValues, string name) :
            this(digitValues, new ConcurrentListChunk<INeuronChunk>(), name)
        {
        }

        public IEnumerableChunk<INeuronChunk> DigitValues => this.Content1;

        public INeuronChunk[] DigitValuesArray
        {
            get
            {
                this.digitValuesArray ??= [.. this.DigitValues.Items];

                return this.digitValuesArray;
            }
        }

        public IConcurrentListChunk<INeuronChunk> Value => this.Content2;

        public INeuronChunk[] ValueArray => 
            this.Value != null ? [.. this.Value.Items] : [];

        public string Name { get; } = name;

        /// <summary>
        /// Clone without value to ensure Value is cleared for each Operation.
        /// Otherwise, Operation will be unable to add to Value since it will be filled 
        /// by a previously instantiated Operation.
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static ResultChunk CloneWithoutValue(ResultChunk original) =>
            new(original.DigitValues, original.Name);
    }
}
