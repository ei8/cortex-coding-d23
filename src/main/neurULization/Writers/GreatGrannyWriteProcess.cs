using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    internal class GreatGrannyWriteProcessAsync<TGranny, TGrannyWriteProcessor, TWriteParameterSet, TResult> : IGreatGrannyProcessAsync<TResult>
        where TGranny : IGranny
        where TGrannyWriteProcessor : IGrannyWriteProcessor<TGranny, TWriteParameterSet>
        where TWriteParameterSet : IWriteParameterSet
    {
        private readonly AsyncGrannyWriteProcessCallback<TGranny, TGrannyWriteProcessor, TWriteParameterSet, TResult> asyncProcess;

        public GreatGrannyWriteProcessAsync(AsyncGrannyWriteProcessCallback<TGranny, TGrannyWriteProcessor, TWriteParameterSet, TResult> asyncProcess)
        {
            this.asyncProcess = asyncProcess;
        }

        public async Task<IGranny> ExecuteAsync(IGreatGrannyInfo<TResult> greatGrannyProcess, Ensemble ensemble, Id23neurULizerOptions options, IGranny precedingGranny, TResult tempResult)
        {
            AssertionConcern.AssertArgumentValid(
                ip => ip is GreatGrannyWriteInfo<TGranny, TGrannyWriteProcessor, TWriteParameterSet, TResult>,
                greatGrannyProcess,
                "Specified parameter must be of compatible generic type.",
                nameof(greatGrannyProcess)
                );

            var typedGreatGrannyProcess = (GreatGrannyWriteInfo<TGranny, TGrannyWriteProcessor, TWriteParameterSet, TResult>)greatGrannyProcess;

            return await asyncProcess(
                typedGreatGrannyProcess.WriteProcessor,
                ensemble,
                options,
                typedGreatGrannyProcess.WriteParametersBuilder(precedingGranny),
                typedGreatGrannyProcess.DerivedGrannyUpdater,
                tempResult
                );
        }
    }

    internal class GreatGrannyWriteProcess<TGranny, TGrannyProcessor, TParameterSet, TResult> : IGreatGrannyProcess<TResult>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyWriteProcessor<TGranny, TParameterSet>
        where TParameterSet : IWriteParameterSet
    {
        private readonly GrannyWriteProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> process;

        public GreatGrannyWriteProcess(GrannyWriteProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> process)
        {
            this.process = process;
        }

        public IGranny Execute(IGreatGrannyInfo<TResult> greatGrannyProcess, Ensemble ensemble, Id23neurULizerOptions options, IGranny precedingGranny, TResult tempResult)
        {
            AssertionConcern.AssertArgumentValid(
                ip => ip is GreatGrannyWriteInfo<TGranny, TGrannyProcessor, TParameterSet, TResult>,
                greatGrannyProcess,
                "Specified parameter must be of compatible generic type.",
                nameof(greatGrannyProcess)
                );

            var typedGreatGrannyProcess = (GreatGrannyWriteInfo<TGranny, TGrannyProcessor, TParameterSet, TResult>)greatGrannyProcess;

            return process(
                typedGreatGrannyProcess.WriteProcessor,
                ensemble,
                options,
                typedGreatGrannyProcess.WriteParametersBuilder(precedingGranny),
                typedGreatGrannyProcess.DerivedGrannyUpdater,
                tempResult
                );
        }
    }
}
