namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IPropertyAssociation : ILesserGranny
    {
    }

    public interface IPropertyAssociation<TPropertyAssignment> : 
        IPropertyAssociation,
        ILesserGranny<TPropertyAssignment>
        where TPropertyAssignment : IGranny
    {
    }
}
