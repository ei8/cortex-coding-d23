namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class Addition
    {
        public class WorkingMemoryInfo
        (
            EnumerableChunk<NeuronChunk> precedingCarryOverValues,
            EnumerableChunk<NeuronChunk> addend1Values,
            EnumerableChunk<NeuronChunk> addend2Values,
            EnumerableChunk<NeuronChunk> sumValues,
            ListChunk<NeuronChunk> sum,
            EnumerableChunk<NeuronChunk> carryOverValues,
            NeuronChunk? carryOver
        ) :
            IWorkingMemory
        {
            public WorkingMemoryInfo
            (
                EnumerableChunk<NeuronChunk> precedingCarryOverValues,
                EnumerableChunk<NeuronChunk> addend1Values,
                EnumerableChunk<NeuronChunk> addend2Values,
                EnumerableChunk<NeuronChunk> sumValues,
                EnumerableChunk<NeuronChunk> carryOverValues
            ) :
                this
                (
                    precedingCarryOverValues,
                    addend1Values,
                    addend2Values,
                    sumValues,
                    new(),
                    carryOverValues,
                    null
                )
            {
            }

            public EnumerableChunk<NeuronChunk> PrecedingCarryOverValues => precedingCarryOverValues;

            public EnumerableChunk<NeuronChunk> Addend1Values => addend1Values;

            public EnumerableChunk<NeuronChunk> Addend2Values => addend2Values;

            public EnumerableChunk<NeuronChunk> SumValues => sumValues;

            public ListChunk<NeuronChunk> Sum => sum;

            public EnumerableChunk<NeuronChunk> CarryOverValues => carryOverValues;

            public NeuronChunk? CarryOver { get; set; } = carryOver;
        }
    }
}
