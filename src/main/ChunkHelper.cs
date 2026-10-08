using ei8.Cortex.Coding.d23.Process;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ei8.Cortex.Coding.d23
{
    public static class ChunkHelper
    {
        public static int? GetOperandCurrentIndex
        (
            NeuronChunk? currentOperand1Digit,
            IEnumerableChunk<NeuronChunk> operand1,
            NeuronChunk? currentOperand2Digit,
            IEnumerableChunk<NeuronChunk> operand2
        )
        {
            int? currentOperand1DigitIndex = currentOperand1Digit != null ?
                operand1.Content.ToList().IndexOf(currentOperand1Digit) :
                null;

            int? currentOperand2DigitIndex = currentOperand2Digit != null ?
                operand2.Content.ToList().IndexOf(currentOperand2Digit) :
                null;

            int? digitIndex = currentOperand1DigitIndex.HasValue && currentOperand2DigitIndex.HasValue ?
                System.Math.Max(currentOperand1DigitIndex.Value, currentOperand2DigitIndex.Value) :
                currentOperand1DigitIndex ?? currentOperand2DigitIndex;

            return digitIndex;
        }

        public static bool TryGetExpectedHandledDigitsCount
        (
            NeuronChunk? currentOperand1Digit,
            IEnumerableChunk<NeuronChunk> operand1,
            NeuronChunk? currentOperand2Digit,
            IEnumerableChunk<NeuronChunk> operand2,
            [NotNullWhen(true)]
            out int? result
        )
        {
            var bResult = false;

            var maxDigitCount = System.Math.Max
            (
                operand1.Content.Count(),
                operand2.Content.Count()
            );

            var currentDigitIndex =
                ChunkHelper.GetOperandCurrentIndex
                (
                    currentOperand1Digit,
                    operand1,
                    currentOperand2Digit,
                    operand2
                );

            if (currentDigitIndex != null)
            {
                bResult = true;
                result = maxDigitCount - currentDigitIndex;
            }
            else
                result = null;

            return bResult;
        }

        public static bool HandleFire
        (
            Neuron target,
            NeuronChunk? currentOperand1Digit,
            IEnumerableChunk<NeuronChunk> operand1,
            NeuronChunk? currentOperand2Digit,
            IEnumerableChunk<NeuronChunk> operand2,
            IEnumerable<ResultInfo> resultInfo,
            out bool allResultsHandled
        )
        {
            bool canHandleCurrentDigit = false;
            allResultsHandled = false;

            if
            (
                ChunkHelper.TryGetExpectedHandledDigitsCount
                (
                    currentOperand1Digit,
                    operand1,
                    currentOperand2Digit,
                    operand2,
                    out var expectedHandledCount
                )
            )
            {
                canHandleCurrentDigit = true;
                allResultsHandled = true;
                foreach (var result in resultInfo)
                    allResultsHandled &= result.HandleFire(target, expectedHandledCount.Value);
            }

            return canHandleCurrentDigit;
        }
    }
}
