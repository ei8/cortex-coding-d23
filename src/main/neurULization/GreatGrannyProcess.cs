using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class GreatGrannyProcess<TGranny, TGrannyProcessor, TParameterSet, TAggregate> : IGreatGrannyProcess<TAggregate>
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
        where TAggregate : IGranny
    {
        private readonly GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TAggregate> process;

        public GreatGrannyProcess(GrannyProcessCallback<TGranny, TGrannyProcessor, TParameterSet, TAggregate> process)
        {
            this.process = process;
        }

        public bool TryGetParameters(
            IGranny precedingGranny,
            IGreatGrannyInfo<TAggregate> greatGrannyInfo,
            out IParameterSet parameters
        )
        {
            parameters = default;

            if (
                greatGrannyInfo is IDependentGreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TAggregate> dependentGreatGrannyInfo &&
                precedingGranny != null
            )
                parameters = dependentGreatGrannyInfo.ParametersBuilder(precedingGranny);
            else if (
                greatGrannyInfo is IIndependentGreatGrannyInfo<TGranny, TGrannyProcessor, TParameterSet, TAggregate> independentGreatGrannyInfo
            )
                parameters = independentGreatGrannyInfo.ParametersBuilder();

            return parameters != default;
        }

        public bool TryExecute(IGreatGrannyInfo<TAggregate> greatGrannyInfo, Network network, TAggregate aggregate, IParameterSet parameters, out IGranny result)
        {
            result = default;
            bool bResult = false;

            if (
                greatGrannyInfo is ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TAggregate> coreGreatGrannyInfo &&
                parameters is TParameterSet tryParameters &&
                process(
                    coreGreatGrannyInfo.Processor,
                    network,
                    tryParameters,
                    coreGreatGrannyInfo.AggregateUpdater,
                    aggregate,
                    out TGranny tempResult
                )
            )
            {
                result = tempResult;
                bResult = true;
            }
            
            return bResult;
        }

        public void UpdateAggregate(IGreatGrannyInfo<TAggregate> greatGrannyInfo, IGranny precedingGranny, TAggregate aggregate)
        {
            if (greatGrannyInfo is ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TAggregate> coreGreatGrannyInfo)
                coreGreatGrannyInfo.AggregateUpdater((TGranny) precedingGranny, aggregate);
        }
    }
}
