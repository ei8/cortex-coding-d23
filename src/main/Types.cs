using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    public delegate IGranny GrannyProcessCallback<TIGranny, TParameterSet, TResult>(
        TIGranny granny, 
        Ensemble ensemble, 
        Id23neurULizerOptions options,
        TParameterSet parameters, 
        Action<TIGranny, TResult> resultUpdater, 
        TResult tempResult
    )
        where TIGranny : IGranny<TIGranny, TParameterSet>
        where TParameterSet : IParameterSet;

    public delegate Task<IGranny> AsyncGrannyProcessCallback<TIGranny, TParameterSet, TResult>(
        TIGranny granny,
        Ensemble ensemble,
        Id23neurULizerOptions options,
        TParameterSet parameters,
        Action<TIGranny, TResult> resultUpdater,
        TResult tempResult
    )
        where TIGranny : IGranny<TIGranny, TParameterSet>
        where TParameterSet : IParameterSet;
}
