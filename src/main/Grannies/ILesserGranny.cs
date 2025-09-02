namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface ILesserGranny : IGranny
    {
        IGranny GetGreatGranny();
    }

    public interface ILesserGranny<TGreatGranny> : ILesserGranny 
        where TGreatGranny : IGranny
    {
        TGreatGranny GreatGranny { get; set; }
    }
}
