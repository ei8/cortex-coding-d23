using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface IGrannyProcessor<TGranny, TParameterSet> 
        where TGranny : IGranny
        where TParameterSet : IParameterSet
    {
    }
}
