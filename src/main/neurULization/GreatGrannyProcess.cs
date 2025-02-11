using ei8.Cortex.Coding.d23.Grannies;

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

        public IGranny Execute(IGreatGrannyInfo<TAggregate> greatGrannyInfo, Network network, TAggregate aggregate, IParameterSet parameters)
        {
            var result = default(IGranny);

            if (greatGrannyInfo is ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TAggregate> coreGreatGrannyInfo &&
                parameters is TParameterSet tryParameters)
            {
                result = process(
                    coreGreatGrannyInfo.Processor,
                    network,
                    tryParameters,
                    coreGreatGrannyInfo.AggregateUpdater,
                    aggregate
                );
            }

            return result;
        }

        public void UpdateAggregate(IGreatGrannyInfo<TAggregate> greatGrannyInfo, IGranny precedingGranny, TAggregate aggregate)
        {
            if (greatGrannyInfo is ICoreGreatGrannyInfo<TGranny, TGrannyProcessor, TAggregate> coreGreatGrannyInfo)
                coreGreatGrannyInfo.AggregateUpdater((TGranny) precedingGranny, aggregate);
        }
    }
}
