using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    public delegate IGranny GrannyProcessCallback<T, TIGranny, TParameterSet, TResult>(
        T granny, 
        Ensemble ensemble, 
        neurULizationOptions options,
        TParameterSet parameters, 
        Action<TIGranny, TResult> resultUpdater, 
        TResult tempResult
    )
        where T : TIGranny, new()
        where TIGranny : IGranny<TIGranny, TParameterSet>
        where TParameterSet : IParameterSet;

    public delegate Task<IGranny> AsyncGrannyProcessCallback<T, TIGranny, TParameterSet, TResult>(
        T granny,
        Ensemble ensemble,
        neurULizationOptions options,
        TParameterSet parameters,
        Action<TIGranny, TResult> resultUpdater,
        TResult tempResult
    )
        where T : TIGranny, new()
        where TIGranny : IGranny<TIGranny, TParameterSet>
        where TParameterSet : IParameterSet;
}
