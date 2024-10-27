using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    internal class InductiveIndependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate> : IInductiveIndependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate>
        where TGranny : IGranny
        where TProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        public InductiveIndependentGreatGrannyInfo(
           Neuron neuron,
           TProcessor processor,
           Func<TParameterSet> parametersBuilder,
           Action<TGranny, TAggregate> aggregateUpdater
           )
        {
            this.Neuron = neuron;
            this.Processor = processor;
            this.ParametersBuilder = parametersBuilder;
            this.AggregateUpdater = aggregateUpdater;
        }

        public Neuron Neuron { get; }
        public TProcessor Processor { get; }
        public Func<TParameterSet> ParametersBuilder { get; }
        public Action<TGranny, TAggregate> AggregateUpdater { get; }
    }
}