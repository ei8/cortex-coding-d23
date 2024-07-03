using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal class InnerProcess<T, TIGranny, TParameterSet, TResult> : IInnerProcess<TResult>
        where T : TIGranny, new()
        where TIGranny : IGranny<TIGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        private readonly T granny;
        private readonly Func<IGranny, TParameterSet> parametersBuilder;
        private readonly Action<TIGranny, TResult> resultUpdater;
        private readonly GrannyProcessCallback<T, TIGranny, TParameterSet, TResult> syncProcess;
        private readonly AsyncGrannyProcessCallback<T, TIGranny, TParameterSet, TResult> asyncProcess;

        public InnerProcess(
           Func<IGranny, TParameterSet> parametersBuilder,
           Action<TIGranny, TResult> resultUpdater,
           GrannyProcessCallback<T, TIGranny, TParameterSet, TResult> syncProcess) : this(parametersBuilder, resultUpdater)
        { 
            this.syncProcess = syncProcess;
        }

        public InnerProcess(
           Func<IGranny, TParameterSet> parametersBuilder,
           Action<TIGranny, TResult> resultUpdater,
           AsyncGrannyProcessCallback<T, TIGranny, TParameterSet, TResult> asyncProcess) : this(parametersBuilder, resultUpdater)
        {
            this.asyncProcess = asyncProcess;
        }

        private InnerProcess(
           Func<IGranny, TParameterSet> parametersBuilder,
           Action<TIGranny, TResult> resultUpdater
           )
        {
            this.granny = new T();
            this.parametersBuilder = parametersBuilder;
            this.resultUpdater = resultUpdater;
        }

        public IGranny Execute(Ensemble ensemble, IPrimitiveSet primitives, IGranny precedingGranny, TResult tempResult)
        {
            AssertionConcern.AssertStateTrue(this.syncProcess != null, "Cannot invoke InnerProcess.Execute() when 'syncProcess' is null.");
            return this.syncProcess(this.granny, ensemble, primitives, this.parametersBuilder(precedingGranny), this.resultUpdater, tempResult);
        }
        public async Task<IGranny> ExecuteAsync(Ensemble ensemble, IPrimitiveSet primitives, IGranny precedingGranny, TResult tempResult)
        {
            AssertionConcern.AssertStateTrue(this.asyncProcess != null, "Cannot invoke InnerProcess.ExecuteAsync() when 'asyncProcess' is null.");
            return await this.asyncProcess(this.granny, ensemble, primitives, this.parametersBuilder(precedingGranny), this.resultUpdater, tempResult);
        }
    }
}