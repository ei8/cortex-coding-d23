using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TGranny"></typeparam>
    /// <typeparam name="TParameterSet"></typeparam>
    public interface IGrannyReader<TGranny, TParameterSet> : IGrannyProcessor<TGranny, TParameterSet>
        where TGranny : IGranny
        where TParameterSet : IParameterSet
    {
        bool TryParse(Network network, TParameterSet parameters, out TGranny result);
    }
}
