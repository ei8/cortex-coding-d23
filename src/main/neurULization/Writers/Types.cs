using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public delegate IGranny GrannyWriteProcessCallback<TGranny, TGrannyWriteProcessor, TParameterWriteSet, TResult>(
        TGrannyWriteProcessor grannyWriteProcessor,
        Ensemble ensemble,
        Id23neurULizerWriteOptions options,
        TParameterWriteSet writeParameters,
        Action<TGranny, TResult> resultUpdater,
        TResult tempResult
    )
        where TGranny : IGranny
        where TGrannyWriteProcessor : IGrannyWriteProcessor<TGranny, TParameterWriteSet>
        where TParameterWriteSet : IWriteParameterSet;

    public delegate Task<IGranny> AsyncGrannyWriteProcessCallback<TGranny, TGrannyWriteProcessor, TWriteParameterSet, TResult>(
        TGrannyWriteProcessor grannyWriteProcessor,
        Ensemble ensemble,
        Id23neurULizerWriteOptions options,
        TWriteParameterSet writeParameters,
        Action<TGranny, TResult> resultUpdater,
        TResult tempResult
    )
        where TGranny : IGranny
        where TGrannyWriteProcessor : IGrannyWriteProcessor<TGranny, TWriteParameterSet>
        where TWriteParameterSet : IWriteParameterSet;
}
