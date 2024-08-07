namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IPropertyValueExpression : IGranny
    {
        IValueExpression ValueExpression { get; set; }

        IExpression Expression { get; set; }
    }
}
