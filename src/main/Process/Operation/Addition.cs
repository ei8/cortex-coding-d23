using ei8.Cortex.Coding.d23.Process.Iteration;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class Addition :
        FiniteCompositeProcessBase
        <
            Addition, 
            Addition.WorkingMemoryInfo, 
            DoUntil
        >
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly string digitPrefix;

        [SetsRequiredMembers]
        public Addition
        (
            WorkingMemoryInfo workingMemory,
            ReadOnlyNeuronChunk action,
            EnumerableChunk digitVariableValues,
            WriteableNeuronChunk digitVariable,
            // TODO: remove digitPrefix and workingMemory.digit1Addends and digit2Addends, use digitRetrievalCallback that returns two digit values based on a specified digit neuron
            string digitPrefix,
            Action<Addition, IProcess?> completionCallback
        ) :
            base
            (
                workingMemory,
                completionCallback
            )
        {
            this.Process1 = new
            (
                new
                (
                    action,
                    digitVariableValues,
                    digitVariable,
                    new(digitVariableValues.Content.Last())
                ),
                null,
                this.Complete
            );

            this.digitPrefix = digitPrefix;
        }

        public override IEnumerable<Neuron> GetCurrent()
        {
            int digitIndex = this.GetCurrentDigitIndex();
            List<Neuron> result = [];

            result.AddRange(this.DoUntil.GetCurrent());

            if (digitIndex == 0)
                result.Add(this.WorkingMemory.PrecedingCarryOverValues.Content.Single(n => n.Tag.EndsWith('0')));
            else if (this.WorkingMemory.CarryOver.Content != null) 
                result.Add(this.WorkingMemory.CarryOver.Content); 

            if (digitIndex < this.WorkingMemory.Addend1Digits.Content.Count())
            {
                result.AddRange
                (
                    [
                        this.WorkingMemory.Addend1Digits.Content.ElementAt(digitIndex),
                        this.WorkingMemory.Addend2Digits.Content.ElementAt(digitIndex)
                    ]
                );
            }
            else if (digitIndex == this.WorkingMemory.Addend1Digits.Content.Count())
            {
                result.AddRange
                (
                    [
                        this.WorkingMemory.Addend1Values.Content.Single(ad => ad.Tag.EndsWith('0')),
                        this.WorkingMemory.Addend2Values.Content.Single(ad => ad.Tag.EndsWith('0'))
                    ]
                );
            }
            else
                this.Complete(null);

            return result;
        }

        private int GetCurrentDigitIndex()
        {
            var currentDigit = this.DoUntil.WorkingMemory.CounterVariable.Value;
            var digitIndex = int.Parse(currentDigit.Tag.ToUpper().Replace(digitPrefix.ToUpper(), string.Empty)) - 1;
            return digitIndex;
        }

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            this.Process1.HandleFire(targetNeuron, network);

            // if one of specified sum values, add to sums
            int currentDigitIndex = this.GetCurrentDigitIndex();
            if
            (
                this.WorkingMemory.SumValues.Content.Contains(targetNeuron) &&
                currentDigitIndex == this.WorkingMemory.Sums.Content.Count
            )
            {
                Addition.logger.Info(new LogMessageGenerator(() => $"Added to Sum(s): {targetNeuron.Tag}"));

                this.WorkingMemory.Sums.Content.Add(targetNeuron);
            }

            // if one of specified carry over values, update carry over
            if(this.WorkingMemory.CarryOverValues.Content.Contains(targetNeuron))
            {
                this.WorkingMemory.CarryOver.Content = 
                    this.WorkingMemory.PrecedingCarryOverValues.Content.Single
                    (
                        n => n.Tag.EndsWith(targetNeuron.Tag.Last())
                    );
            }
        }

        private void Complete(IProcess? process)
        {
            Addition.logger.Info
            (
                new LogMessageGenerator(() => $"Sum: {string.Join("", this.WorkingMemory.Sums.Content.Reverse().Select(s => s.Tag.Last()))}")
            );

            this.completionCallback(this, process);
        }

        public DoUntil DoUntil => this.Process1;
    }
}
