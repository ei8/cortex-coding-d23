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
            IOperandChunk operand1,
            IOperandChunk operand2
        )
        {
            ArgumentNullException.ThrowIfNull(operand1.Value);
            ArgumentNullException.ThrowIfNull(operand2.Value);

            int? currentOperand1DigitIndex = operand1.CurrentDigit != null ?
                operand1.Value.Items.ToList().IndexOf(operand1.CurrentDigit) :
                null;

            int? currentOperand2DigitIndex = operand2.CurrentDigit != null ?
                operand2.Value.Items.ToList().IndexOf(operand2.CurrentDigit) :
                null;

            int? digitIndex = currentOperand1DigitIndex.HasValue && currentOperand2DigitIndex.HasValue ?
                System.Math.Max(currentOperand1DigitIndex.Value, currentOperand2DigitIndex.Value) :
                currentOperand1DigitIndex ?? currentOperand2DigitIndex;

            return digitIndex;
        }

        public static bool TryGetExpectedHandledDigitsCount
        (
            IOperandChunk operand1,
            IOperandChunk operand2,
            bool incremental,
            [NotNullWhen(true)]
            out int? result
        )
        {
            ArgumentNullException.ThrowIfNull(operand1.Value);
            ArgumentNullException.ThrowIfNull(operand2.Value);

            var bResult = false;

            var currentDigitIndex =
                ChunkHelper.GetOperandCurrentIndex
                (
                    operand1,
                    operand2
                );

            if (currentDigitIndex != null)
            {
                bResult = true;
                if (incremental)
                    result = currentDigitIndex + 1;
                else
                {
                    var maxDigitCount = System.Math.Max
                    (
                        operand1.Value.Items.Count(),
                        operand2.Value.Items.Count()
                    );
                    result = maxDigitCount - currentDigitIndex;
                }
            }
            else
                result = null;

            return bResult;
        }

        public static bool HandleFire
        (
            Neuron target,
            IOperandChunk operand1,
            IOperandChunk operand2,
            bool incremental,
            IEnumerable<IResultChunk> resultInfo,
            [NotNullWhen(true)]
            out bool? allResultsHandled
        )
        {
            bool canHandleCurrentDigit = false;
            allResultsHandled = null;
            bool? allResultsHandledTemp = null;

            if
            (
                ChunkHelper.TryGetExpectedHandledDigitsCount
                (
                    operand1,
                    operand2,
                    incremental,
                    out var expectedHandledCount
                )
            )
            {
                canHandleCurrentDigit = true;
                foreach (var result in resultInfo)
                {
                    if (allResultsHandled == null)
                        allResultsHandled = result.HandleFire(target, expectedHandledCount.Value);
                    else
                        allResultsHandled &= result.HandleFire(target, expectedHandledCount.Value);
                }
                allResultsHandledTemp = allResultsHandled;
            }

            return canHandleCurrentDigit;
        }
    }
}
