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

            if
            (
                this.operands.Any(o => o.HasCurrentDigitChanged()) &&
                this.WorkingMemory.TryHandleAllResults
                (
                    targetNeuron,
                    this.IsDigitProgressionIncremental,
                    out result
                )
            )
            {
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