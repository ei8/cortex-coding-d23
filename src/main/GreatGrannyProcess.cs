using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using neurUL.Common.Domain.Model;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal class GreatGrannyProcessAsync<TGranny, TGrannyProcessor, TParameterSet, TResult> : IGreatGrannyProcessAsync<TResult>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        private readonly AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> asyncProcess;

        public GreatGrannyProcessAsync(AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> asyncProcess)
        {
            this.asyncProcess = asyncProcess;
        }

        public async Task<IGranny> ExecuteAsync(IGreatGrannyInfo<TResult> greatGrannyProcess, Ensemble ensemble, Id23neurULizerOptions options, IGranny precedingGranny, TResult tempResult)
        {
            AssertionConcern.AssertArgumentValid(
                ip => ip is GreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TResult>,
                greatGrannyProcess,
                "Specified parameter must be of compatible generic type.",
                nameof(greatGrannyProcess)
                );

            var typedGreatGrannyProcess = (GreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TResult>) greatGrannyProcess;

            return await this.asyncProcess(
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
    {
        private readonly GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> process;

        public GreatGrannyProcess(GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> process)
        {
            this.process = process;
        }

        public IGranny Execute(IGreatGrannyInfo<TResult> greatGrannyProcess, Ensemble ensemble, Id23neurULizerOptions options, IGranny precedingGranny, TResult tempResult)
        {
            AssertionConcern.AssertArgumentValid(
                ip => ip is GreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TResult>,
                greatGrannyProcess,
                "Specified parameter must be of compatible generic type.",
                nameof(greatGrannyProcess)
                );

            var typedGreatGrannyProcess = (GreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TResult>) greatGrannyProcess;

            return this.process(
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
