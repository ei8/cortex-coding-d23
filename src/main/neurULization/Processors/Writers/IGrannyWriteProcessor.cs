using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TGranny"></typeparam>
    /// <typeparam name="TParameterSet"></typeparam>
    public interface IGrannyWriteProcessor<TGranny, TParameterSet> : IGrannyProcessor<TGranny, TParameterSet>
        where TGranny : IGranny
        where TParameterSet : IDeductiveParameterSet
    {
        Task<TGranny> BuildAsync(Ensemble ensemble, TParameterSet parameters);

        IGrannyReadProcessor<TGranny, TParameterSet> ReadProcessor { get; }
    }
}
