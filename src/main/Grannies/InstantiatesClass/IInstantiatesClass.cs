namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IInstantiatesClass : IGranny<IInstantiatesClass, IInstantiatesClassParameterSet>
    {
        IDependent Class { get; }
    }
}
