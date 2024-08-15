using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    internal class GreatGrannyReadInfo<T, TReadProcessor, TReadParameterSet, TDerivedGranny> : IGreatGrannyInfo<TDerivedGranny>
        where T : IGranny
        where TReadProcessor : IGrannyReadProcessor<T, TReadParameterSet>
        where TReadParameterSet : IReadParameterSet
    {
        public GreatGrannyReadInfo(
           TReadProcessor readProcessor,
           TReadParameterSet readParameters,
           Action<T, TDerivedGranny> derivedGrannyUpdater
           )
        {
            ReadProcessor = readProcessor;
            ReadParameters = readParameters;
            DerivedGrannyUpdater = derivedGrannyUpdater;
        }

        public TReadProcessor ReadProcessor { get; }
        public TReadParameterSet ReadParameters { get; }
        public Action<T, TDerivedGranny> DerivedGrannyUpdater { get; }
    }
}