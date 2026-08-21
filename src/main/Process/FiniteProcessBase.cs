namespace ei8.Cortex.Coding.d23.Process
{
    public abstract class FiniteProcessBase<T>(T workingMemory) :
        ProcessBase<T>(workingMemory),
        IFinite
        where T : IWorkingMemory
    {
        // TODO: check if possible refactor with FiniteCompositeProcessBase
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
