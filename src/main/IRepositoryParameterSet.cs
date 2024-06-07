namespace ei8.Cortex.Coding.d23
{
    public interface IRepositoryParameterSet : IParameterSet
    {
        IEnsembleRepository NeuronRepository { get; }

        string UserId { get; }
    }
}
