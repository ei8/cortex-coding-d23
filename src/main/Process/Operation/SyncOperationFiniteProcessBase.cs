using System;
using System.Collections.Generic;
using System.Linq;

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

            if (this.WorkingMemory.GetOperands().TryGetCurrentDigitIndex(out var digitIndex))
                result.AddRange(this.WorkingMemory.GetCurrent(digitIndex.Value).Select(c => c.Neuron));
            else
                this.Complete();

            return result;
        }

        public override bool HandleFire(Neuron targetNeuron)
        {
            bool result = false;

            int? currentDigitIndex = null;
            var operandChunks = this.WorkingMemory.GetOperands();
            if
            (
                this.WorkingMemory.TryHandleAllResults
                (
                    targetNeuron,
                    (out count) =>
                    {
                        count = null;
                        if (operandChunks.TryGetCurrentDigitIndex(out currentDigitIndex))
                        {
                            count =
                                operandChunks.GetExpectedHandledDigitsCount
                                (
                                    currentDigitIndex.Value,
                                    this.IsDigitProgressionIncremental
                                );
                        }
                        return count.HasValue;
                    },
                    out var targetHandled
                ) &&
                targetHandled
            )
            {
                result = true;
                if 
                (
                    operandChunks.TryGetCurrentDigitIndex(out var innerCurrentDigitIndex) &&
                    innerCurrentDigitIndex == currentDigitIndex
                )
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