namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IInstantiates : IGranny<IInstantiates, IInstantiatesParameterSet>
    {
        Neuron Subordination { get; }
        Neuron InstantiatesUnit { get; }
        IDependency ClassDirectObject { get; }
    }
}
