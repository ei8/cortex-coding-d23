using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class Addition
    {
        public class WorkingMemoryValuesInfo
        (
            EnumerableChunk<NeuronChunk> precedingCarryOverValues,
            EnumerableChunk<NeuronChunk> augendValues,
            EnumerableChunk<NeuronChunk> addendValues,
            EnumerableChunk<NeuronChunk> sumValues,
            EnumerableChunk<NeuronChunk> carryOverValues
        ) :
            IWorkingMemory
        {
            public EnumerableChunk<NeuronChunk> PrecedingCarryOverValues => precedingCarryOverValues;

            public EnumerableChunk<NeuronChunk> AugendValues => augendValues;

            public EnumerableChunk<NeuronChunk> AddendValues => addendValues;

            public EnumerableChunk<NeuronChunk> SumValues => sumValues;

            public EnumerableChunk<NeuronChunk> CarryOverValues => carryOverValues;
        }

        public class WorkingMemoryInfo
        (
            EnumerableChunk<NeuronChunk> precedingCarryOverValues,
            EnumerableChunk<NeuronChunk> augendValues,
            EnumerableChunk<NeuronChunk> addendValues,
            EnumerableChunk<NeuronChunk> augend,
            EnumerableChunk<NeuronChunk> addend,
            NeuronChunk? currentAugendDigit,
            NeuronChunk? currentAddendDigit,
            NeuronChunk? lastAugendDigit,
            NeuronChunk? lastAddendDigit,
            EnumerableChunk<NeuronChunk> sumValues,
            ListChunk<NeuronChunk> sum,
            EnumerableChunk<NeuronChunk> carryOverValues,
            NeuronChunk? carryOver
        ) :
            WorkingMemoryValuesInfo
            (
                precedingCarryOverValues,
                augendValues,
                addendValues, 
                sumValues,
                carryOverValues
            )
        {
            public WorkingMemoryInfo
            (
                EnumerableChunk<NeuronChunk> precedingCarryOverValues,
                EnumerableChunk<NeuronChunk> augendValues,
                EnumerableChunk<NeuronChunk> addendValues,
                EnumerableChunk<NeuronChunk> augend,
                EnumerableChunk<NeuronChunk> addend,
                EnumerableChunk<NeuronChunk> sumValues,
                EnumerableChunk<NeuronChunk> carryOverValues
            ) :
                this
                (
                    precedingCarryOverValues,
                    augendValues,
                    addendValues,
                    augend,
                    addend,
                    augend.Content.First(),
                    addend.Content.First(),
                    null,
                    null,
                    sumValues,
                    new(),
                    carryOverValues,
                    null
                )
            {
            }

            public EnumerableChunk<NeuronChunk> Augend => augend;

            public EnumerableChunk<NeuronChunk> Addend => addend;

            public NeuronChunk? CurrentAugendDigit { get; set; } = currentAugendDigit;

            public NeuronChunk? CurrentAddendDigit { get; set; } = currentAddendDigit;

            public NeuronChunk? LastAugendDigit { get; set; } = lastAugendDigit;

            public NeuronChunk? LastAddendDigit { get; set; } = lastAddendDigit;

            public ListChunk<NeuronChunk> Sum => sum;

            public NeuronChunk? CarryOver { get; set; } = carryOver;
        }
    }
}
