using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class LessThan
    {
        public class WorkingMemoryValuesInfo
        (
            EnumerableChunk<NeuronChunk> comparand1Values,
            EnumerableChunk<NeuronChunk> comparand2Values,
            EnumerableChunk<NeuronChunk> resultValues
        ) :
            IWorkingMemory
        {
            public EnumerableChunk<NeuronChunk> Comparand1Values => comparand1Values;

            public EnumerableChunk<NeuronChunk> Comparand2Values => comparand2Values;

            public EnumerableChunk<NeuronChunk> ResultValues => resultValues;
        }

        public class WorkingMemoryInfo
        (
            EnumerableChunk<NeuronChunk> comparand1Values,
            EnumerableChunk<NeuronChunk> comparand2Values,
            EnumerableChunk<NeuronChunk> comparand1,
            EnumerableChunk<NeuronChunk> comparand2,
            NeuronChunk? currentComparand1Digit,
            NeuronChunk? currentComparand2Digit,
            NeuronChunk? previousComparand1Digit,
            NeuronChunk? previousComparand2Digit,
            EnumerableChunk<NeuronChunk> resultValues,
            ListChunk<NeuronChunk> result
        ) :
            WorkingMemoryValuesInfo
            (
                comparand1Values,
                comparand2Values,
                resultValues
            )
        {
            public WorkingMemoryInfo
            (
                EnumerableChunk<NeuronChunk> comparand1Values,
                EnumerableChunk<NeuronChunk> comparand2Values,
                EnumerableChunk<NeuronChunk> comparand1,
                EnumerableChunk<NeuronChunk> comparand2,
                EnumerableChunk<NeuronChunk> resultValues
            ) :
                this
                (
                    comparand1Values,
                    comparand2Values,
                    comparand1,
                    comparand2,
                    comparand1.Content.Last(),
                    comparand2.Content.Last(),
                    null,
                    null,
                    resultValues,
                    new()
                )
            {
            }

            public void AddCurrent(IList<Neuron> result, int digitIndex)
            {
                Addition.WorkingMemoryInfo.AddAddend(digitIndex, result, this.Comparand1Values, this.Comparand1);
                Addition.WorkingMemoryInfo.AddAddend(digitIndex, result, this.Comparand2Values, this.Comparand2);
            }

            public EnumerableChunk<NeuronChunk> Comparand1 => comparand1;

            public EnumerableChunk<NeuronChunk> Comparand2 => comparand2;

            public NeuronChunk? CurrentComparand1Digit { get; set; } = currentComparand1Digit;

            public NeuronChunk? CurrentComparand2Digit { get; set; } = currentComparand2Digit;

            public NeuronChunk? PreviousComparand1Digit { get; set; } = previousComparand1Digit;

            public NeuronChunk? PreviousComparand2Digit { get; set; } = previousComparand2Digit;

            public ListChunk<NeuronChunk> Result => result;
        }
    }
}
