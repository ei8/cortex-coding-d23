using ei8.Cortex.Coding.d23.Grannies;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal class GreatGrannyProcessAsync<TGranny, TGrannyProcessor, TParameterSet, TResult> : IGreatGrannyProcessAsync<TResult>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
        where TResult : IGranny
    {
        private readonly AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> asyncProcess;

        public GreatGrannyProcessAsync(AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> asyncProcess)
        {
            this.asyncProcess = asyncProcess;
        }

        public async Task<IGranny> ExecuteAsync(IGreatGrannyInfo<TResult> greatGrannyProcess, Ensemble ensemble, Id23neurULizerOptions options, IGranny precedingGranny, TResult tempResult)
        {
            var typedGreatGrannyProcess = (GreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TResult>)greatGrannyProcess;

            return await asyncProcess(
                typedGreatGrannyProcess.Processor,
                ensemble,
                options,
                typedGreatGrannyProcess.ParametersBuilder(precedingGranny),
                typedGreatGrannyProcess.DerivedGrannyUpdater,
                tempResult
                );
        }
    }

    internal class GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TResult> : IGreatGrannyProcess<TResult>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
        where TResult : IGranny
    {
        private readonly GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> process;

        public GreatGrannyProcess(GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> process)
        {
            this.process = process;
        }

        public IGranny Execute(IGreatGrannyInfo<TResult> greatGrannyProcess, Ensemble ensemble, Id23neurULizerOptions options, IGranny precedingGranny, TResult tempResult)
        {
            var typedGreatGrannyProcess = (GreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TResult>)greatGrannyProcess;

            return process(
                typedGreatGrannyProcess.Processor,
                ensemble,
                options,
                typedGreatGrannyProcess.ParametersBuilder(precedingGranny),
                typedGreatGrannyProcess.DerivedGrannyUpdater,
                tempResult
                );
        }
    }
}
