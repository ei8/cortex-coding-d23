using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class DependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate> : IDependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate>
        where TGranny : IGranny
        where TProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        public DependentGreatGrannyInfo(
           TProcessor processor,
           Func<IGranny, TParameterSet> parametersBuilder,
           Action<TGranny, TAggregate> aggregateUpdater
           )
        {
            this.Processor = processor;
            this.ParametersBuilder = parametersBuilder;
            this.AggregateUpdater = aggregateUpdater;
        }

        public TProcessor Processor { get; }
        public Func<IGranny, TParameterSet> ParametersBuilder { get; }
        public Action<TGranny, TAggregate> AggregateUpdater { get; }
    }
}