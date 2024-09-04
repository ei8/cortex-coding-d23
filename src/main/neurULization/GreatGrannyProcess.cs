using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal class GreatGrannyProcessAsync<TGranny, TGrannyProcessor, TParameterSet, TResult> : IGreatGrannyProcessAsync<TResult>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : class, IParameterSet
        where TResult : IGranny
    {
        private readonly AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> asyncProcess;

        public GreatGrannyProcessAsync(AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> asyncProcess)
        {
            this.asyncProcess = asyncProcess;
        }

        public async Task<IGranny> ExecuteAsync(IGreatGrannyInfo<TResult> greatGrannyInfo, Ensemble ensemble, Id23neurULizerOptions options, IGranny precedingGranny, TResult tempResult)
        {
            var result = default(IGranny);

            if (GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TResult>.TryGetParameters(
                precedingGranny,
                greatGrannyInfo,
                out TParameterSet parameters,
                out ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TResult> coreGreatGrannyInfo
            ))
                result = await asyncProcess(
                        coreGreatGrannyInfo.Processor,
                        ensemble,
                        options,
                        parameters,
                        coreGreatGrannyInfo.DerivedGrannyUpdater,
                        tempResult
                    );

            return result;
        }
    }

    internal class GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TResult> : IGreatGrannyProcess<TResult>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : class, IParameterSet
        where TResult : IGranny
    {
        private readonly GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> process;

        public GreatGrannyProcess(GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TResult> process)
        {
            this.process = process;
        }

        public IGranny Execute(IGreatGrannyInfo<TResult> greatGrannyInfo, Ensemble ensemble, Id23neurULizerOptions options, IGranny precedingGranny, TResult tempResult)
        {
            var result = default(IGranny);

            if (TryGetParameters(
                precedingGranny, 
                greatGrannyInfo, 
                out TParameterSet parameters,
                out ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TResult> coreGreatGrannyInfo
            ))
                result = process(
                    coreGreatGrannyInfo.Processor,
                    ensemble,
                    options,
                    parameters,
                    coreGreatGrannyInfo.DerivedGrannyUpdater,
                    tempResult
                );

            return result;
        }

        internal static bool TryGetParameters(
            IGranny precedingGranny, 
            IGreatGrannyInfo<TResult> greatGrannyInfo, 
            out TParameterSet parameters,
            out ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TResult> resultCoreGreatGrannyInfo
            )

        {
            parameters = default;
            resultCoreGreatGrannyInfo = null;

            if (greatGrannyInfo is ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TResult> coreGreatGrannyInfo)
            {
                resultCoreGreatGrannyInfo = coreGreatGrannyInfo;

                if (
                    resultCoreGreatGrannyInfo is IDependentGreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TResult> dependentGreatGrannyInfo &&
                    precedingGranny != null
                )
                    parameters = dependentGreatGrannyInfo.ParametersBuilder(precedingGranny);
                else if (
                    resultCoreGreatGrannyInfo is IIndependentGreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TResult> independentGreatGrannyInfo
                )
                    parameters = independentGreatGrannyInfo.ParametersBuilder();
            }

            GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TResult>.LogPropertyName(parameters);

            return parameters != default && resultCoreGreatGrannyInfo != null;
        }

        [Conditional("DEBUG")]
        private static void LogPropertyName(TParameterSet parameters)
        {
            if (parameters is IPropertyReadParameterSet prop)
                Debug.WriteLine($">>> Property Name: {prop.Property.Tag}");
        }
    }
}
