using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using neurUL.Common.Domain.Model;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal class InnerProcess<TGranny, TGrannyProcessor, TParameterSet, TResult> : IInnerProcess<TResult>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        private readonly TGrannyProcessor grannyProcessor;
        private readonly Func<IGranny, TParameterSet> parametersBuilder;
        private readonly Action<TGranny, TResult> resultUpdater;
        private readonly GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> syncProcess;
        private readonly AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> asyncProcess;

        public InnerProcess(
            TGrannyProcessor grannyProcessor,
            Func<IGranny, TParameterSet> parametersBuilder,
            Action<TGranny, TResult> resultUpdater,
            GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> syncProcess) : this(grannyProcessor, parametersBuilder, resultUpdater)
        { 
            this.syncProcess = syncProcess;
        }

        public InnerProcess(
            TGrannyProcessor grannyProcessor,
            Func<IGranny, TParameterSet> parametersBuilder,
            Action<TGranny, TResult> resultUpdater,
            AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> asyncProcess) : this(grannyProcessor, parametersBuilder, resultUpdater)
        {
            this.asyncProcess = asyncProcess;
        }

        private InnerProcess(
           TGrannyProcessor grannyProcessor,
           Func<IGranny, TParameterSet> parametersBuilder,
           Action<TGranny, TResult> resultUpdater
           )
        {
            this.grannyProcessor = grannyProcessor;
            this.parametersBuilder = parametersBuilder;
            this.resultUpdater = resultUpdater;
        }

        public IGranny Execute(
            Ensemble ensemble, 
            Id23neurULizerOptions options, 
            IGranny precedingGranny, 
            TResult tempResult
            )
        {
            AssertionConcern.AssertStateTrue(this.syncProcess != null, "Cannot invoke InnerProcess.Execute() when 'syncProcess' is null.");
            return this.syncProcess(
                this.grannyProcessor, 
                ensemble, 
                options, 
                this.parametersBuilder(precedingGranny), 
                this.resultUpdater, 
                tempResult
                );
        }
        public async Task<IGranny> ExecuteAsync(
            Ensemble ensemble, 
            Id23neurULizerOptions options, 
            IGranny precedingGranny, 
            TResult tempResult
            )
        {
            AssertionConcern.AssertStateTrue(this.asyncProcess != null, "Cannot invoke InnerProcess.ExecuteAsync() when 'asyncProcess' is null.");
            return await this.asyncProcess(
                this.grannyProcessor, 
                ensemble, 
                options, 
                this.parametersBuilder(precedingGranny), 
                this.resultUpdater, 
                tempResult
                );
        }
    }
}