namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class FiniteCompositeProcessBase<TWorkingMemory, TProcess>(TWorkingMemory workingMemory, TProcess process) :
        CompositeProcessBase<TWorkingMemory, TProcess>(workingMemory, process),
        IFinite
        where TWorkingMemory : IWorkingMemory
        where TProcess : IProcess
    {
        // TODO: check if possible refactor with FiniteProcessBase
        public FiniteStatus Status { get; protected set; }

        public bool IsCompleted { get; protected set; }

        protected void Complete()
        {
            this.StopCore(true);
        }

        protected virtual void ResetTransientMemory() { }

        protected virtual void Start()
        {
            this.ResetTransientMemory();
            this.Status = FiniteStatus.InProgress;
            this.IsCompleted = false;
        }

        protected virtual void StopCore(bool isCompleted)
        {
            this.Status = FiniteStatus.Idle;
            this.IsCompleted = isCompleted;
        }

        public void Stop()
        {
            this.StopCore(false);
        }
    }
}
