using System;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process
{
    public class OperandChunk
    (
        IEnumerableChunk<INeuronChunk> digitValues,
        IEnumerableChunk<INeuronChunk>? value,
        INeuronChunk? previousDigit,
        INeuronChunk? currentDigit
    ) :
        AggregateChunkBase
        <
            IEnumerableChunk<INeuronChunk>,
            IEnumerableChunk<INeuronChunk>?,
            INeuronChunk?,
            INeuronChunk?
        >
        (
            digitValues,
            value,
            previousDigit,
            currentDigit
        ),
        IOperandChunk
    {
        private INeuronChunk[]? digitValuesArray;

        private INeuronChunk[]? valueArray;

        public OperandChunk
        (
            IEnumerableChunk<INeuronChunk> digitValues
        ) :
            this
            (
                digitValues,
                null,
                null,
                null
            )
        {
        }

        public OperandChunk
        (
            IEnumerableChunk<INeuronChunk> digitValues,
            IEnumerableChunk<INeuronChunk> value
        ) :
            this
            (
                digitValues,
                value,
                null,
                value.Items.First()
            )
        {
        }

        public OperandChunk
        (
            IEnumerableChunk<INeuronChunk> digitValues,
            IEnumerableChunk<INeuronChunk>? value,
            INeuronChunk? currentDigit
        ) :
            this
            (
                digitValues,
                value,
                null,
                currentDigit
            )
        {
        }

        public IEnumerableChunk<INeuronChunk> DigitValues => this.Content1;

        public INeuronChunk[] DigitValuesArray
        {
            get
            {
                this.digitValuesArray ??= [.. this.DigitValues.Items];

                return this.digitValuesArray;
            }
        }

        public IEnumerableChunk<INeuronChunk>? Value => this.Content2;

        public INeuronChunk? CurrentDigit
        {
            get => this.Content4;
            private set
            {
                this.Content4 = value;
            }
        }

        public INeuronChunk[]? ValueArray
        {
            get
            {
                if (this.Value != null && this.valueArray == null)
                    this.valueArray = [.. this.Value.Items];

                return this.valueArray;
            }
        }

        public int? GetCurrentDigitIndex()
        {
            int? result = null;

            var va = this.ValueArray;
            if (va != null)
                result = Array.IndexOf(va, this.CurrentDigit);

            return result;
        }

        public void ResetTracking()
        {
            ArgumentNullException.ThrowIfNull(this.Value);

            this.CurrentDigit = this.Value.Items.First();
        }

        public void UpdateTracking(bool next)
        {
            ArgumentNullException.ThrowIfNull(this.Value);

            var va = this.ValueArray;
            if (va != null)
                this.CurrentDigit = va.GetAdjacentOrDefault(this.CurrentDigit, next);
        }
    }
}
