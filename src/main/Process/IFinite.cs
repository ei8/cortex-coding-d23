namespace ei8.Cortex.Coding.d23.Process
{
    public enum FiniteStatus
    {
        Idle,
        InProgress
    }
    
    public interface IFinite
    {
        void Stop();

        FiniteStatus Status { get; }

        bool IsCompleted { get; }
    }
}
