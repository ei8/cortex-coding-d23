namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface ILesserGranny<TGreatGranny> : IGranny
    {
        TGreatGranny GreatGranny { get; set; }
    }
}
