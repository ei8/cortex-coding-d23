namespace ei8.Cortex.Coding.d23.Grannies
{
    /// <summary>
    /// Exposes parameters required by Aggregate grannies or grannies that contain other grannies.
    /// </summary>
    public interface IAggregateParameterSet : IParameterSet
    {
        IEnsembleRepository NeuronRepository { get; }

        string UserId { get; }
    }
}
