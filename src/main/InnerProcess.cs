using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23
{
    internal class InnerProcess<TGranny, TGrannyProcessor, TParameterSet, TResult> : IInnerProcess<TResult>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        public InnerProcess(
           TGrannyProcessor grannyProcessor,
           Func<IGranny, TParameterSet> parametersBuilder,
           Action<TGranny, TResult> resultUpdater
           )
        {
            this.GrannyProcessor = grannyProcessor;
            this.ParametersBuilder = parametersBuilder;
            this.ResultUpdater = resultUpdater;
        }

        public TGrannyProcessor GrannyProcessor { get; }
        public Func<IGranny, TParameterSet> ParametersBuilder { get; }
        public Action<TGranny, TResult> ResultUpdater { get; }
    }
}