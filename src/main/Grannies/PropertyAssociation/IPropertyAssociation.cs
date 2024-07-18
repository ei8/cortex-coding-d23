namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IPropertyAssociation : IGranny
    {
        IPropertyAssignment PropertyAssignment { get; set; }

        IExpression Expression { get; set; }
    }
}
