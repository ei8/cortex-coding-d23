using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using neurUL.Common.Domain.Model;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal class InnerProcessTargetAsync<TGranny, TGrannyProcessor, TParameterSet, TResult> : IInnerProcessTargetAsync<TResult>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        private readonly AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> asyncProcess;

        public InnerProcessTargetAsync(AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> asyncProcess)
        {
            this.asyncProcess = asyncProcess;
        }

        public async Task<IGranny> ExecuteAsync(IInnerProcess<TResult> innerProcess, Ensemble ensemble, Id23neurULizerOptions options, IGranny precedingGranny, TResult tempResult)
        {
            AssertionConcern.AssertArgumentValid(
                ip => ip is InnerProcess<TGranny, TGrannyProcessor, TParameterSet, TResult>,
                innerProcess,
                "Specified parameter must be of compatible generic type.",
                nameof(innerProcess)
                );

            var typedInnerProcess = (InnerProcess<TGranny, TGrannyProcessor, TParameterSet, TResult>)innerProcess;

            return await this.asyncProcess(
                typedInnerProcess.GrannyProcessor,
                ensemble,
                options,
                typedInnerProcess.ParametersBuilder(precedingGranny),
                typedInnerProcess.ResultUpdater,
                tempResult
                );
        }
    }

    internal class InnerProcessTarget<TGranny, TGrannyProcessor, TParameterSet, TResult> : IInnerProcessTarget<TResult>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        private readonly GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> process;

        public InnerProcessTarget(GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> process)
        {
            this.process = process;
        }

        public IGranny Execute(IInnerProcess<TResult> innerProcess, Ensemble ensemble, Id23neurULizerOptions options, IGranny precedingGranny, TResult tempResult)
        {
            AssertionConcern.AssertArgumentValid(
                ip => ip is InnerProcess<TGranny, TGrannyProcessor, TParameterSet, TResult>,
                innerProcess,
                "Specified parameter must be of compatible generic type.",
                nameof(innerProcess)
                );

            var typedInnerProcess = (InnerProcess<TGranny, TGrannyProcessor, TParameterSet, TResult>)innerProcess;

            return this.process(
                typedInnerProcess.GrannyProcessor,
                ensemble,
                options,
                typedInnerProcess.ParametersBuilder(precedingGranny),
                typedInnerProcess.ResultUpdater,
                tempResult
                );
        }
    }
}
