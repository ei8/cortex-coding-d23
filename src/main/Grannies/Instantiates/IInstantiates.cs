namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IInstantiates : IGranny<IInstantiates, IInstantiatesParameterSet>
    {
        IDependent ClassDirectObject { get; }
    }
}
