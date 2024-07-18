using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using neurUL.Common.Domain.Model;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal class InnerProcess<TIGranny, TParameterSet, TResult> : IInnerProcess<TResult>
        where TIGranny : IGranny<TIGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        private readonly TIGranny granny;
        private readonly Func<IGranny, TParameterSet> parametersBuilder;
        private readonly Action<TIGranny, TResult> resultUpdater;
        private readonly GrannyProcessCallback<TIGranny, TParameterSet, TResult> syncProcess;
        private readonly AsyncGrannyProcessCallback<TIGranny, TParameterSet, TResult> asyncProcess;

        public InnerProcess(
            TIGranny granny,
            Func<IGranny, TParameterSet> parametersBuilder,
            Action<TIGranny, TResult> resultUpdater,
            GrannyProcessCallback<TIGranny, TParameterSet, TResult> syncProcess) : this(granny, parametersBuilder, resultUpdater)
        { 
            this.syncProcess = syncProcess;
        }

        public InnerProcess(
            TIGranny granny,
            Func<IGranny, TParameterSet> parametersBuilder,
            Action<TIGranny, TResult> resultUpdater,
            AsyncGrannyProcessCallback<TIGranny, TParameterSet, TResult> asyncProcess) : this(granny, parametersBuilder, resultUpdater)
        {
            this.asyncProcess = asyncProcess;
        }

        private InnerProcess(
           TIGranny granny,
           Func<IGranny, TParameterSet> parametersBuilder,
           Action<TIGranny, TResult> resultUpdater
           )
        {
            this.granny = granny;
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
                this.granny, 
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
                this.granny, 
                ensemble, 
                options, 
                this.parametersBuilder(precedingGranny), 
                this.resultUpdater, 
                tempResult
                );
        }
    }
}