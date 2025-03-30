namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface ILesserGranny : IGranny
    {
    }

    public interface ILesserGranny<TGreatGranny> : ILesserGranny
    {
        TGreatGranny GreatGranny { get; set; }
    }
}
