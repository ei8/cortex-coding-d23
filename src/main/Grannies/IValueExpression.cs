namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IValueExpression : IGranny
    {
        IValue Value { get; set; }

        IExpression Expression { get; set; }
    }
}
