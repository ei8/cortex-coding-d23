﻿using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    public delegate IGranny GrannyProcessCallback<T, TIGranny, TParameterSet, TResult>(
        T granny, 
        Ensemble ensemble, 
        IPrimitiveSet primitives, 
        TParameterSet parameters, 
        Action<TIGranny, TResult> resultUpdater, 
        TResult tempResult,
        IEnsembleRepository ensembleRepository,
        string userId
    )
        where T : TIGranny, new()
        where TIGranny : IGranny<TIGranny, TParameterSet>
        where TParameterSet : IParameterSet;

    public delegate Task<IGranny> AsyncGrannyProcessCallback<T, TIGranny, TParameterSet, TResult>(
        T granny,
        Ensemble ensemble,
        IPrimitiveSet primitives,
        TParameterSet parameters,
        Action<TIGranny, TResult> resultUpdater,
        TResult tempResult,
        IEnsembleRepository ensembleRepository,
        string userId
    )
        where T : TIGranny, new()
        where TIGranny : IGranny<TIGranny, TParameterSet>
        where TParameterSet : IParameterSet;
}
