namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IPropertyAssignment : IGranny
    {
        IPropertyValueExpression PropertyValueExpression { get; set; }

        IExpression Expression { get; set; }
    }
}
