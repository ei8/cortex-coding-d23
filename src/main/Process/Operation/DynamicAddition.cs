using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class DynamicAddition
    (
        Addition.WorkingMemoryInfo workingMemory,
        Action<DynamicAddition, IEnumerable<Neuron>> completionCallback
    ) :
        FiniteProcessBase
        <
            Addition.WorkingMemoryInfo,
            Action<DynamicAddition, IEnumerable<Neuron>>
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
            // if one of specified sum values, add to sums
            if
            (
                this.WorkingMemory.SumValues.Content.Any(c => c.Value == targetNeuron) &&
                (
                    (
                        this.WorkingMemory.LastAugendDigit != this.WorkingMemory.CurrentAugendDigit &&
                        this.WorkingMemory.CurrentAugendDigit != null
                    ) ||
                    (
                        this.WorkingMemory.LastAddendDigit != this.WorkingMemory.CurrentAugendDigit &&
                        this.WorkingMemory.CurrentAddendDigit != null
                    )
                ) 
            )
            {
                if (this.WorkingMemory.LastAugendDigit != this.WorkingMemory.CurrentAugendDigit)
                    this.WorkingMemory.LastAugendDigit = this.WorkingMemory.CurrentAugendDigit;

                if (this.WorkingMemory.LastAddendDigit != this.WorkingMemory.CurrentAddendDigit)
                    this.WorkingMemory.LastAddendDigit = this.WorkingMemory.CurrentAddendDigit;

                this.WorkingMemory.Sum.Content.Add(new(targetNeuron));

                DynamicAddition.logger.Info(new LogMessageGenerator(() => $"Added to Sum(s): {targetNeuron.Tag}"));

                if
                (
                    (
                        this.WorkingMemory.CurrentAugendDigit = 
                            this.WorkingMemory.Augend.Content.IncrementReset(this.WorkingMemory.CurrentAugendDigit)
                    ) == null &&
                    (
                        this.WorkingMemory.CurrentAddendDigit =
                            this.WorkingMemory.Addend.Content.IncrementReset(this.WorkingMemory.CurrentAddendDigit)
                    ) == null
                )
                    this.Complete();
            }

            // if one of specified carry over values, update carry over
            if(this.WorkingMemory.CarryOverValues.Content.Any(c => c.Value == targetNeuron))
            {
                this.WorkingMemory.CarryOver = 
                    this.WorkingMemory.PrecedingCarryOverValues.Content.Single
                    (
                        n => n.Value.Tag.EndsWith(targetNeuron.Tag.Last())
                    );
            }
        }

        private void Complete()
        {
            this.completionCallback(this, [.. this.WorkingMemory.GetSumNeurons()]);
            this.WorkingMemory.Sum.Content.Clear();
        }
    }
}
