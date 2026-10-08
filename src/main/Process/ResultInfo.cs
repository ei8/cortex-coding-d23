using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Process
{
    public class ResultInfo
    (
        IEnumerableChunk<NeuronChunk> values,
        IListChunk<NeuronChunk> list,
        [CallerArgumentExpression(nameof(list))] string name = ""
    )
    {
        public IEnumerableChunk<NeuronChunk> Values { get; } = values;
        public IListChunk<NeuronChunk> List { get; } = list;
        public string Name { get; } = name;
    }
}
