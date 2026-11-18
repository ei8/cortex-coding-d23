using ei8.Cortex.Coding.d23.Process.Iteration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class SequentialAddition :
        FiniteCompositeProcessBase
        <
            Addition.WorkingMemoryInfo, 
            DoUntil,
            Action<SequentialAddition, IProcess?, IEnumerable<Neuron>>
        >
    {
        private readonly Func<Neuron, int?> digitIndexRetriever;

        public SequentialAddition
        (
            Addition.WorkingMemoryInfo workingMemory,
            DoUntil.WorkingMemoryInfo doUntilWorkingMemory,
            Func<Neuron, int?> digitIndexRetriever,
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

            this.digitIndexRetriever = digitIndexRetriever;
        }

        public override IEnumerable<Neuron> GetCurrent()
        {
            ArgumentNullException.ThrowIfNull(this.DoUntil);

            List<Neuron> result = [];

            result.AddRange(this.DoUntil.GetCurrent());

            var counterIndex = this.digitIndexRetriever(this.DoUntil.WorkingMemory.CounterVariable.Neuron);
            if (counterIndex.HasValue)
                result.AddRange(this.WorkingMemory.GetCurrent(counterIndex.Value).Select(c => c.Neuron));
            else
                this.Complete(null);

            return result;
        }

        public override bool HandleFire(Neuron targetNeuron)
        {
            ArgumentNullException.ThrowIfNull(this.DoUntil);

            var result = false;
            if 
            (
                this.WorkingMemory.TryHandleAllResults
                (
                    targetNeuron,
                    (wm) =>
                    {
                        var result = this.digitIndexRetriever(this.DoUntil.WorkingMemory.CounterVariable.Neuron);

                        if (result != null)
                            result++;

                        return result;
                    },
                    out var targetHandled
                ) &&
                this.DoUntil.HandleFire(targetNeuron)
            )
            {
                result = true;
            }

            return result;
        }

        private void Complete(IProcess? process)
        {
            this.completionCallback
            (
                this, 
                process,
                Addition.WorkingMemoryInfo.GetResultNeuronns(this.WorkingMemory)
            );
            this.WorkingMemory.Sum.Value.Clear();
        }

        public DoUntil? DoUntil => this.Process1;
    }
}
