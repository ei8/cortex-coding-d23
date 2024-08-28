using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal class IndependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TDerivedGranny> : IIndependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TDerivedGranny>
        where TGranny : IGranny
        where TProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        public IndependentGreatGrannyInfo(
           TProcessor processor,
           Func<TParameterSet> parametersBuilder,
           Action<TGranny, TDerivedGranny> derivedGrannyUpdater
           )
        {
            this.Processor = processor;
            this.ParametersBuilder = parametersBuilder;
            this.DerivedGrannyUpdater = derivedGrannyUpdater;
        }

        public TProcessor Processor { get; }
        public Func<TParameterSet> ParametersBuilder { get; }
        public Action<TGranny, TDerivedGranny> DerivedGrannyUpdater { get; }
    }
}