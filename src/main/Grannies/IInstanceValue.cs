namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IInstanceValue : IExpressionGranny, ILesserGranny<IInstantiatesClass>
    {
        IUnit Value { get; set; }
    }
}
