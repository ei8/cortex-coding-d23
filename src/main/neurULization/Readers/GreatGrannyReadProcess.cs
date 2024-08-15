using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    internal class GreatGrannyReadProcess<TGranny, TGrannyReadProcessor, TReadParameterSet, TResult> : IGreatGrannyReadProcess<TResult>
        where TGranny : IGranny
        where TGrannyReadProcessor : IGrannyReadProcessor<TGranny, TReadParameterSet>
        where TReadParameterSet : IReadParameterSet
    {
        private readonly GrannyReadProcessCallback<TGranny, TGrannyReadProcessor, TReadParameterSet, TResult> process;

        public GreatGrannyReadProcess(GrannyReadProcessCallback<TGranny, TGrannyReadProcessor, TReadParameterSet, TResult> process)
        {
            this.process = process;
        }

        public IGranny Execute(IGreatGrannyInfo<TResult> greatGrannyProcess, Ensemble ensemble, Id23neurULizerOptions options, TResult tempResult)
        {
            AssertionConcern.AssertArgumentValid(
                ip => ip is GreatGrannyReadInfo<TGranny, TGrannyReadProcessor, TReadParameterSet, TResult>,
                greatGrannyProcess,
                "Specified parameter must be of compatible generic type.",
                nameof(greatGrannyProcess)
                );

            var typedGreatGrannyProcess = (GreatGrannyReadInfo<TGranny, TGrannyReadProcessor, TReadParameterSet, TResult>)greatGrannyProcess;

            return process(
                typedGreatGrannyProcess.ReadProcessor,
                ensemble,
                (Id23neurULizerReadOptions) options,
                typedGreatGrannyProcess.ReadParameters,
                typedGreatGrannyProcess.DerivedGrannyUpdater,
                tempResult
                );
        }
    }
}
