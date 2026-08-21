using ei8.Cortex.Coding.d23.Process.Iteration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class Addition
    (
        Addition.WorkingMemoryInfo workingMemory,
        DoUntil doUntil,
        string digitPrefix
    ) :
        FiniteCompositeProcessBase<Addition.WorkingMemoryInfo, DoUntil>
        (
            workingMemory,
            doUntil
        )
    {
        public override IEnumerable<Neuron> GetCurrent()
        {
            int digitIndex = GetCurrentDigitIndex();
            List<Neuron> result = new(
                [
                    this.WorkingMemory.DoUntil.Action.Value,
                    this.WorkingMemory.DoUntil.CounterVariable.Value,
                    this.WorkingMemory.Addend1Digits.Content.ElementAt(digitIndex),
                    this.WorkingMemory.Addend2Digits.Content.ElementAt(digitIndex)
                ]
            );
            if (this.WorkingMemory.CarryOver.Content != null)
                result.Add(this.WorkingMemory.CarryOver.Content);

            return result;
        }

        protected override void ResetTransientMemory()
        {
            base.ResetTransientMemory();

            this.WorkingMemory.CarryOver.Value = null;
        }

        private int GetCurrentDigitIndex()
        {
            var currentDigit = this.WorkingMemory.DoUntil.CounterVariable.Value;
            var digitIndex = int.Parse(currentDigit.Tag.ToUpper().Replace(digitPrefix.ToUpper(), string.Empty)) - 1;
            return digitIndex;
        }

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            base.HandleFire(targetNeuron, network);

            this.Process.HandleFire(targetNeuron, network);

            // if one of specified sum values, add to sums
            if
            (
                this.WorkingMemory.SumValues.Content.Contains(targetNeuron) &&
                this.GetCurrentDigitIndex() == this.WorkingMemory.Sums.Content.Count
            )
                this.WorkingMemory.Sums.Content.Add(targetNeuron);

            // if one of specified carry over values, update carry over
            if(this.WorkingMemory.CarryOverValues.Content.Contains(targetNeuron))
                this.WorkingMemory.CarryOver.Value = targetNeuron;

            if (this.Process.IsCompleted)
                this.Complete();
        }

        private string digitPrefix = digitPrefix;

        public DoUntil DoUntil => this.Process;
    }
}
