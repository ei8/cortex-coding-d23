using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public delegate IGranny GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult>(
        TGrannyProcessor grannyProcessor,
        Ensemble ensemble,
        TParameterSet parameters,
        Action<TGranny, TResult> resultUpdater,
        TResult tempResult
    )
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
        where TResult : IGranny;

    public delegate Task<IGranny> AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult>(
        TGrannyProcessor grannyProcessor,
        Ensemble ensemble,
        TParameterSet parameters,
        Action<TGranny, TResult> resultUpdater,
        TResult tempResult
    )
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
        where TResult : IGranny;
}
