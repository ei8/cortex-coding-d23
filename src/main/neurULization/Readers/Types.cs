using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    public delegate IGranny GrannyReadProcessCallback<TGranny, TGrannyReadProcessor, TParameterReadSet, TResult>(
        TGrannyReadProcessor grannyReadProcessor,
        Ensemble ensemble,
        Id23neurULizerReadOptions options,
        TParameterReadSet readParameters,
        Action<TGranny, TResult> resultUpdater,
        TResult tempResult
    )
        where TGranny : IGranny
        where TGrannyReadProcessor : IGrannyReadProcessor<TGranny, TParameterReadSet>
        where TParameterReadSet : IReadParameterSet;
}
