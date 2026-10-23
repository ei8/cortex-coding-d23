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

        public IEnumerableChunk<INeuronChunk>? Value => this.Content2;

        public INeuronChunk? PreviousDigit
        {
            get => this.Content3;
            private set
            {
                this.Content3 = value;
            }
        }

        public INeuronChunk? CurrentDigit
        {
            get => this.Content4;
            private set
            {
                this.Content4 = value;
            }
        }

        public void ResetTracking()
        {
            ArgumentNullException.ThrowIfNull(this.Value);

            this.PreviousDigit = null;
            this.CurrentDigit = this.Value.Items.First();
        }

        public void UpdateTracking(bool next)
        {
            ArgumentNullException.ThrowIfNull(this.Value);

            this.PreviousDigit = this.CurrentDigit;
            this.CurrentDigit = this.Value.Items.GetAdjacentOrDefault(this.CurrentDigit, next);
        }
    }
}
