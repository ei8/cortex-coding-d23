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
                    null,
                    null,
                    null,
                    null,
                    resultValues,
                    new()
                )
            {
            }

            public bool TryAddCurrent(IList<Neuron> result) =>
                this.TryAddCurrent(result, this.Result.Content.Count);

            public bool TryAddCurrent(IList<Neuron> result, int digitIndex)
            {
                var bResult = true;

                if
                (
                    digitIndex > this.Comparand1.Content.Count() &&
                    digitIndex > this.Comparand2.Content.Count()
                )
                    bResult = false;
                else
                {
                    var maxDigitCount = System.Math.Max
                        (
                            this.Comparand1.Content.Count(),
                            this.Comparand2.Content.Count()
                        );
                    WorkingMemoryInfo.AddComparand(digitIndex, result, this.Comparand1Values, this.Comparand1, maxDigitCount);
                    WorkingMemoryInfo.AddComparand(digitIndex, result, this.Comparand2Values, this.Comparand2, maxDigitCount);
                }

                return bResult;
            }

            private static void AddComparand
            (
                int digitIndex, 
                IList<Neuron> comparandsResult, 
                EnumerableChunk<NeuronChunk> comparandValues, 
                EnumerableChunk<NeuronChunk> comparand,
                int maxDigitCount
            )
            {
                var diffDigit = maxDigitCount - comparand.Content.Count();
                if (diffDigit > 0)
                {
                    if (digitIndex < diffDigit)
                        comparandsResult.Add(comparandValues.Content.Single(ad => ad.Value.Tag.EndsWith('0')).Value);
                    else
                    {
                        comparandsResult.Add
                        (
                            comparandValues.Content.GetByComparer(
                                nc => nc.IsMatchingValue
                                    (
                                        comparand,
                                        comparand.Content.Count() + diffDigit - 1 - digitIndex
                                    )
                            ).Value
                        );
                    }
                }
                else
                {
                    comparandsResult.Add
                    (
                        comparandValues.Content.GetByComparer(
                            nc => nc.IsMatchingValue
                                (
                                    comparand,
                                    comparand.Content.Count() - 1 - digitIndex
                                )
                        ).Value
                    );
                }
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
