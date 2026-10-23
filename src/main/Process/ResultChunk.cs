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
        public ResultChunk(IEnumerableChunk<INeuronChunk> digitValues, string name) :
            this(digitValues, new ConcurrentListChunk<INeuronChunk>(), name)
        {
        }

        public IEnumerableChunk<INeuronChunk> DigitValues => this.Content1;

        public IConcurrentListChunk<INeuronChunk> Value => this.Content2;

        public string Name { get; } = name;

        public static ResultChunk CloneWithoutValue(ResultChunk original) =>
            new(original.DigitValues, original.Name);
    }
}
