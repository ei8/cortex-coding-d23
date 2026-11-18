using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public abstract class SyncOperationFiniteProcessBase
    <
        T,
        TWorkingMemory
    > :
        FiniteProcessBase
        <
            TWorkingMemory,
            Action<T, IEnumerable<Neuron>>
        >
        where T : IProcess<TWorkingMemory>
        where TWorkingMemory : ISyncOperationWorkingMemory
    {
        private IEnumerable<IOperandChunk> operands;
        public SyncOperationFiniteProcessBase
        (
            TWorkingMemory workingMemory,
            Action<T, IEnumerable<Neuron>> completionCallback
        ) : 
            base
            (
                workingMemory,
                completionCallback
            )
        {
            this.operands = this.WorkingMemory.Chunks.OfType<IOperandChunk>();
        }

        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];
            int? digitIndex = this.WorkingMemory.GetOperandsCurrentIndex();

            if (digitIndex.HasValue)
                result.AddRange(this.WorkingMemory.GetCurrent(digitIndex.Value).Select(c => c.Neuron));
            else
                this.Complete();

            return result;
        }

        public override bool HandleFire(Neuron targetNeuron)
        {
            bool result = false;

            int? currentDigitIndex = null;
            if
            (
                this.WorkingMemory.TryHandleAllResults
                (
                    targetNeuron,
                    (wm) =>
                    {
                        int? result = null;
                        currentDigitIndex = this.WorkingMemory.GetOperandsCurrentIndex();
                        if (currentDigitIndex != null)
                        {
                            result =
                                wm.GetExpectedHandledDigitsCount
                                (
                                    currentDigitIndex.Value,
                                    this.IsDigitProgressionIncremental
                                );
                        }
                        return result;
                    },
                    out var targetHandled
                ) &&
                targetHandled
            )
            {
                result = true;
                if (this.WorkingMemory.GetOperandsCurrentIndex() == currentDigitIndex)
                    foreach (var operand in this.operands)
                        operand.UpdateTracking(this.IsTrackingDirectionNext);

                if(this.IsComplete())
                    this.Complete();
            }

            return result;
        }

        protected virtual bool IsComplete() =>
            this.operands.All(o => o.CurrentDigit == null);

        protected abstract bool IsTrackingDirectionNext { get; }

        protected abstract bool IsDigitProgressionIncremental { get; }

        protected abstract IEnumerable<Neuron> BuildResults();

        private void Complete()
        {
            this.completionCallback((T)(object)this, this.BuildResults());

            foreach (var result in WorkingMemory.Chunks.OfType<IResultChunk>())
                result.Value.Clear();
        }
    }
}