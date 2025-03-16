namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface ILesserGranny : IGranny
    {
        IGranny GreatGranny { get; set; }
    }

    public interface ILesserGranny<TGreatGranny> : ILesserGranny
    {
        TGreatGranny TypedGreatGranny { get; set; }
    }
}
