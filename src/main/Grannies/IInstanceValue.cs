namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IInstanceValue : IValue
    {
        IUnit Value { get; set; }

        IInstantiatesClass InstantiatesClass { get; set; }

        IExpression Expression { get; set; }
    }
}
