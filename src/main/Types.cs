using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    public delegate IGranny GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult>(
        TGrannyProcessor granny, 
        Ensemble ensemble, 
        Id23neurULizerOptions options,
        TParameterSet parameters, 
        Action<TGranny, TResult> resultUpdater, 
        TResult tempResult
    )
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet;

    public delegate Task<IGranny> AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult>(
        TGrannyProcessor granny,
        Ensemble ensemble,
        Id23neurULizerOptions options,
        TParameterSet parameters,
        Action<TGranny, TResult> resultUpdater,
        TResult tempResult
    )
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet;
}
