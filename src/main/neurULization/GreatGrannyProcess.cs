using ei8.Cortex.Coding.d23.Grannies;
using System.Diagnostics;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal class GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TAggregate> : IGreatGrannyProcess<TAggregate>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : class, IParameterSet
        where TAggregate : IGranny
    {
        private readonly GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TAggregate> process;

        public GreatGrannyProcess(GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TAggregate> process)
        {
            this.process = process;
        }

        public IGranny Execute(IGreatGrannyInfo<TAggregate> greatGrannyInfo, Ensemble ensemble, IGranny precedingGranny, TAggregate aggregate)
        {
            var result = default(IGranny);

            if (GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TAggregate>.TryGetParameters(
                precedingGranny, 
                greatGrannyInfo, 
                out TParameterSet parameters,
                out ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TAggregate> coreGreatGrannyInfo
            ))
                result = process(
                    coreGreatGrannyInfo.Processor,
                    ensemble,
                    parameters,
                    coreGreatGrannyInfo.AggregateUpdater,
                    aggregate
                );

            return result;
        }

        public void UpdateAggregate(
            IGreatGrannyInfo<TAggregate> greatGrannyInfo, 
            IGranny precedingGranny, 
            TAggregate aggregate
        )
        {
            if (GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TAggregate>.TryGetParameters(
                precedingGranny,
                greatGrannyInfo,
                out TParameterSet parameters,
                out ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TAggregate> coreGreatGrannyInfo
            ))
                coreGreatGrannyInfo.AggregateUpdater((TGranny) precedingGranny, aggregate);
        }

        internal static bool TryGetParameters(
            IGranny precedingGranny, 
            IGreatGrannyInfo<TAggregate> greatGrannyInfo, 
            out TParameterSet parameters,
            out ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TAggregate> resultCoreGreatGrannyInfo
        )
        {
            parameters = default;
            resultCoreGreatGrannyInfo = null;

            if (greatGrannyInfo is ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TAggregate> coreGreatGrannyInfo)
            {
                resultCoreGreatGrannyInfo = coreGreatGrannyInfo;

                if (
                    resultCoreGreatGrannyInfo is IDependentGreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TAggregate> dependentGreatGrannyInfo &&
                    precedingGranny != null
                )
                    parameters = dependentGreatGrannyInfo.ParametersBuilder(precedingGranny);
                else if (
                    resultCoreGreatGrannyInfo is IIndependentGreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TAggregate> independentGreatGrannyInfo
                )
                    parameters = independentGreatGrannyInfo.ParametersBuilder();
            }

            GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TAggregate>.LogPropertyName(parameters);

            return parameters != default && resultCoreGreatGrannyInfo != null;
        }

        [Conditional("DEBUG")]
        private static void LogPropertyName(TParameterSet parameters)
        {
            if (parameters is Processors.Readers.Inductive.IPropertyReadParameterSet prop)
                Debug.WriteLine($">>> Property Name: {prop.Property.Tag}");
        }
    }
}
