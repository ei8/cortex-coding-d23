namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IDependency : IGranny<IDependency, IDependencyParameterSet>
    {
        Neuron Value { get; }
        Neuron Type { get; }
    }
}
