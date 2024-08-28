using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal class DependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TDerivedGranny> : IDependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TDerivedGranny>
        where TGranny : IGranny
        where TProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        public DependentGreatGrannyInfo(
           TProcessor processor,
           Func<IGranny, TParameterSet> parametersBuilder,
           Action<TGranny, TDerivedGranny> derivedGrannyUpdater
           )
        {
            this.Processor = processor;
            this.ParametersBuilder = parametersBuilder;
            this.DerivedGrannyUpdater = derivedGrannyUpdater;
        }

        public TProcessor Processor { get; }
        public Func<IGranny, TParameterSet> ParametersBuilder { get; }
        public Action<TGranny, TDerivedGranny> DerivedGrannyUpdater { get; }
    }
}