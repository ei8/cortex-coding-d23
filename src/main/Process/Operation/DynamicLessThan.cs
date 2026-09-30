using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public class DynamicLessThan
    (
        LessThan.WorkingMemoryInfo workingMemory,
        Action<DynamicLessThan, IEnumerable<Neuron>> completionCallback
    ) :
        FiniteProcessBase
        <
            LessThan.WorkingMemoryInfo,
            Action<DynamicLessThan, IEnumerable<Neuron>>
        >
        (
            workingMemory,
            completionCallback
        )
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];

            if (!this.WorkingMemory.TryAddCurrent(result))
                this.Complete();

            return result;
        }

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            // if one of specified result values, add to results
            if
            (
                this.WorkingMemory.ResultValues.Content.Any(c => c.Value == targetNeuron) &&
                (
                    this.WorkingMemory.PreviousComparand1Digit.NextExists(this.WorkingMemory.CurrentComparand1Digit) ||
                    this.WorkingMemory.PreviousComparand2Digit.NextExists(this.WorkingMemory.CurrentComparand2Digit)
                )
            )
            {
                this.WorkingMemory.PreviousComparand1Digit = this.WorkingMemory.PreviousComparand1Digit.GetIfUnequal(this.WorkingMemory.CurrentComparand1Digit);
                this.WorkingMemory.PreviousComparand2Digit = this.WorkingMemory.PreviousComparand2Digit.GetIfUnequal(this.WorkingMemory.CurrentComparand2Digit);

                this.WorkingMemory.Result.Content.Add(new(targetNeuron));

                DynamicLessThan.logger.Info(new LogMessageGenerator(() => $"Added to Result(s): {targetNeuron.Tag}"));

                if
                (
                    (
                        this.WorkingMemory.CurrentComparand1Digit =
                            this.WorkingMemory.Comparand1.Content.IncrementReset(this.WorkingMemory.CurrentComparand1Digit)
                    ) == null &&
                    (
                        this.WorkingMemory.CurrentComparand2Digit =
                            this.WorkingMemory.Comparand2.Content.IncrementReset(this.WorkingMemory.CurrentComparand2Digit)
                    ) == null
                )
                    this.Complete();
            }
        }

        private void Complete()
        {
            this.completionCallback(this, this.WorkingMemory.Result.Content.Select(c => c.Value));
            this.WorkingMemory.Result.Content.Clear();
        }
    }
}
