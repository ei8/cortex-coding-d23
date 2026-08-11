namespace ei8.Cortex.Coding.d23.Process
{
    public interface IProcess
    {
        void Start(ISpikable spikable, IWorkingMemory workingMemory);

        void Stop();
    }

    public interface IProcess<T> : IProcess
       where T : notnull
    {
        void Start(ISpikable spikable, IWorkingMemory<T> workingMemory);
    }
}
