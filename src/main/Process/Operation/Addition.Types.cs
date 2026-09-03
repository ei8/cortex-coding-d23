using System.Collections.Generic;
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

            public IEnumerable<Neuron> GetSumNeurons()
            {
                var result = new List<Neuron>(this.Sum.Content.Select(c => c.Value));

                if (this.CarryOver != null && this.CarryOver.Value.Tag.EndsWith('1'))
                    result.Add(this.CarryOver.Value);

                return result;
            }

            public bool TryAddCurrent(IList<Neuron> result) =>
                this.TryAddCurrent(result, this.Sum.Content.Count);

            public bool TryAddCurrent(IList<Neuron> result, int digitIndex)
            {
                var bResult = true;

                if (digitIndex == 0)
                    result.Add(this.PrecedingCarryOverValues.Content.Single(n => n.Value.Tag.EndsWith('0')).Value);
                else if (this.CarryOver != null)
                    result.Add(this.CarryOver.Content);

                if
                (
                    digitIndex > this.Augend.Content.Count() &&
                    digitIndex > this.Addend.Content.Count()
                )
                    bResult = false;
                else
                {
                    WorkingMemoryInfo.AddAddend(digitIndex, result, this.AugendValues, this.Augend);
                    WorkingMemoryInfo.AddAddend(digitIndex, result, this.AddendValues, this.Addend);
                }

                return bResult;
            }

            private static void AddAddend(int digitIndex, IList<Neuron> addendsResult, EnumerableChunk<NeuronChunk> addendValues, EnumerableChunk<NeuronChunk> addend)
            {
                if (digitIndex < addend.Content.Count())
                    addendsResult.Add(addendValues.Content.Single(ad => ad.Value.Tag.EndsWith(addend.Content.ElementAt(digitIndex).Value.Tag.Last())).Value);
                else
                    addendsResult.Add(addendValues.Content.Single(ad => ad.Value.Tag.EndsWith('0')).Value);
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
