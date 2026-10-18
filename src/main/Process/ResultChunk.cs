using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Process
{
    public class ResultChunk
    (
        IEnumerableChunk<INeuronChunk> digitValues,
        IListChunk<INeuronChunk> value,
        [CallerArgumentExpression(nameof(value))] string name = ""
    ) :
        AggregateChunkBase
        <
            IEnumerableChunk<INeuronChunk>,
            IListChunk<INeuronChunk>
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
            this(digitValues, new ListChunk<INeuronChunk>(), name)
        {
        }

        public IEnumerableChunk<INeuronChunk> DigitValues => this.Content1;

        public IListChunk<INeuronChunk> Value => this.Content2;

        public string Name { get; } = name;

        public static ResultChunk CloneWithoutValue(ResultChunk original) =>
            new(original.DigitValues, original.Name);
    }
}
