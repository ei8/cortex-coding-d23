using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23
{
    internal class GreatGrannyInfo<T, TProcessor, TParameterSet, TDerivedGranny> : IGreatGrannyInfo<TDerivedGranny>
        where T : IGranny
        where TProcessor : IGrannyProcessor<T, TParameterSet>
        where TParameterSet : IParameterSet
    {
        public GreatGrannyInfo(
           TProcessor processor,
           Func<IGranny, TParameterSet> parametersBuilder,
           Action<T, TDerivedGranny> derivedGrannyUpdater
           )
        {
            this.Processor = processor;
            this.ParametersBuilder = parametersBuilder;
            this.DerivedGrannyUpdater = derivedGrannyUpdater;
        }

        public TProcessor Processor { get; }
        public Func<IGranny, TParameterSet> ParametersBuilder { get; }
        public Action<T, TDerivedGranny> DerivedGrannyUpdater { get; }
    }
}