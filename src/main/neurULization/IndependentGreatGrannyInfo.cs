using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class IndependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate> : IIndependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate>
        where TGranny : IGranny
        where TProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        public IndependentGreatGrannyInfo(
           TProcessor processor,
           Func<TParameterSet> parametersBuilder,
           Action<TGranny, TAggregate> aggregateUpdater
           )
        {
            this.Processor = processor;
            this.ParametersBuilder = parametersBuilder;
            this.AggregateUpdater = aggregateUpdater;
        }

        public TProcessor Processor { get; }
        public Func<TParameterSet> ParametersBuilder { get; }
        public Action<TGranny, TAggregate> AggregateUpdater { get; }
    }
}