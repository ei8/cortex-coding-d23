using ei8.Cortex.Coding.d23.Process.Iteration;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class SequentialAddition :
        FiniteCompositeProcessBase
        <
            SequentialAddition, 
            Addition.WorkingMemoryInfo, 
            DoUntil,
            Action<SequentialAddition, IProcess?, IEnumerable<Neuron>>
        >
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Func<Neuron, int> digitRetriever;
        private Neuron? lastSumDigit;

        [SetsRequiredMembers]
        public SequentialAddition
        (
            Addition.WorkingMemoryInfo workingMemory,
            DoUntil.WorkingMemoryInfo doUntilWorkingMemory,
            Func<Neuron, int> digitRetriever,
            Action<SequentialAddition, IProcess?, IEnumerable<Neuron>> completionCallback
        ) :
            base
            (
                workingMemory,
                completionCallback
            )
        {
            this.Process1 = new
            (
                doUntilWorkingMemory,
                null,
                this.Complete
            );

            this.digitRetriever = digitRetriever;
        }

        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];

            if (this.DoUntil != null)
            {
                result.AddRange(this.DoUntil.GetCurrent());

                var digitIndex = this.digitRetriever(this.DoUntil.WorkingMemory.CounterVariable.Value);
                if (!SequentialAddition.AddAddends(result, digitIndex, this.WorkingMemory))
                    this.Complete(null);
            }

            return result;
        }

        internal static bool AddAddends(List<Neuron> result, int digitIndex, Addition.WorkingMemoryInfo workingMemory)
        {
            var bResult = true;

            if (digitIndex == 0)
                result.Add(workingMemory.PrecedingCarryOverValues.Content.Single(n => n.Value.Tag.EndsWith('0')).Value);
            else if (workingMemory.CarryOver != null)
                result.Add(workingMemory.CarryOver.Content);

            if
            (
                digitIndex > workingMemory.Augend.Content.Count() &&
                digitIndex > workingMemory.Addend.Content.Count()
            )
                bResult = false;
            else
            {
                SequentialAddition.AddAddend(digitIndex, result, workingMemory.AugendValues, workingMemory.Augend);
                SequentialAddition.AddAddend(digitIndex, result, workingMemory.AddendValues, workingMemory.Addend);
            }

            return bResult;
        }

        // TODO: transfer to helper function
        private static void AddAddend(int digitIndex, List<Neuron> addendsResult, EnumerableChunk<NeuronChunk> addendValues, EnumerableChunk<NeuronChunk> addend)
        {
            if (digitIndex < addend.Content.Count())
                addendsResult.Add(addendValues.Content.Single(ad => ad.Value.Tag.EndsWith(addend.Content.ElementAt(digitIndex).Value.Tag.Last())).Value);
            else
                addendsResult.Add(addendValues.Content.Single(ad => ad.Value.Tag.EndsWith('0')).Value);
        }

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            if (this.DoUntil != null)
            {
                this.DoUntil.HandleFire(targetNeuron, network);

                // if one of specified sum values, add to sums
                if
                (
                    this.WorkingMemory.SumValues.Content.Any(c => c.Value == targetNeuron) &&
                    lastSumDigit != this.DoUntil.WorkingMemory.CounterVariable.Value
                )
                {
                    SequentialAddition.logger.Info(new LogMessageGenerator(() => $"Added to Sum(s): {targetNeuron.Tag}"));

                    this.lastSumDigit = this.DoUntil.WorkingMemory.CounterVariable.Value;
                    this.WorkingMemory.Sum.Content.Add(new(targetNeuron));
                }
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

        private void Complete(IProcess? process)
        {
            List<Neuron> result = SequentialAddition.GetSum
            (
                this.WorkingMemory.Sum.Content.Select(c => c.Value), 
                this.WorkingMemory.CarryOver?.Value
            );

            this.completionCallback(this, process, [.. result]);
            this.WorkingMemory.Sum.Content.Clear();
        }

        internal static List<Neuron> GetSum(IEnumerable<Neuron> sums, Neuron? carryOver)
        {
            var result = new List<Neuron>(sums);

            if (carryOver != null)
                result.Add(carryOver);

            return result;
        }

        public DoUntil? DoUntil => this.Process1;
    }
}
