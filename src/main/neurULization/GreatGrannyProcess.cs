using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal class GreatGrannyProcessAsync<TGranny, TGrannyProcessor, TParameterSet, TAggregate> : IGreatGrannyProcessAsync<TAggregate>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : class, IParameterSet
        where TAggregate : IGranny
    {
        private readonly AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TAggregate> asyncProcess;

        public GreatGrannyProcessAsync(AsyncGrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TAggregate> asyncProcess)
        {
            this.asyncProcess = asyncProcess;
        }

        public async Task<IGranny> ExecuteAsync(IGreatGrannyInfo<TAggregate> greatGrannyInfo, Ensemble ensemble, IGranny precedingGranny, TAggregate aggregate)
        {
            var result = default(IGranny);

            if (GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TAggregate>.TryGetParameters(
                precedingGranny,
                greatGrannyInfo,
                out TParameterSet parameters,
                out ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TAggregate> coreGreatGrannyInfo
            ))
                result = await asyncProcess(
                        coreGreatGrannyInfo.Processor,
                        ensemble,
                        parameters,
                        coreGreatGrannyInfo.AggregateUpdater,
                        aggregate
                    );

            return result;
        }
    }

    internal class GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TAggregateResult> : IGreatGrannyProcess<TAggregateResult>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : class, IParameterSet
        where TAggregateResult : IGranny
    {
        private readonly GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TAggregateResult> process;

        public GreatGrannyProcess(GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TAggregateResult> process)
        {
            this.process = process;
        }

        public IGranny Execute(IGreatGrannyInfo<TAggregateResult> greatGrannyInfo, Ensemble ensemble, IGranny precedingGranny, TAggregateResult tempResult)
        {
            var result = default(IGranny);

            if (GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TAggregateResult>.TryGetParameters(
                precedingGranny, 
                greatGrannyInfo, 
                out TParameterSet parameters,
                out ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TAggregateResult> coreGreatGrannyInfo
            ))
                result = process(
                    coreGreatGrannyInfo.Processor,
                    ensemble,
                    parameters,
                    coreGreatGrannyInfo.AggregateUpdater,
                    tempResult
                );

            return result;
        }

        internal static bool TryGetParameters(
            IGranny precedingGranny, 
            IGreatGrannyInfo<TAggregateResult> greatGrannyInfo, 
            out TParameterSet parameters,
            out ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TAggregateResult> resultCoreGreatGrannyInfo
            )

        {
            parameters = default;
            resultCoreGreatGrannyInfo = null;

            if (greatGrannyInfo is ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TAggregateResult> coreGreatGrannyInfo)
            {
                resultCoreGreatGrannyInfo = coreGreatGrannyInfo;

                if (
                    resultCoreGreatGrannyInfo is IDependentGreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TAggregateResult> dependentGreatGrannyInfo &&
                    precedingGranny != null
                )
                    parameters = dependentGreatGrannyInfo.ParametersBuilder(precedingGranny);
                else if (
                    resultCoreGreatGrannyInfo is IIndependentGreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TAggregateResult> independentGreatGrannyInfo
                )
                    parameters = independentGreatGrannyInfo.ParametersBuilder();
            }

            GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TAggregateResult>.LogPropertyName(parameters);

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
