using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal interface ICoreGreatGrannyInfo<TGranny, TProcessor, TDerivedGranny> : IGreatGrannyInfo<TDerivedGranny>
        where TGranny : IGranny
    {
        TProcessor Processor { get; }
        Action<TGranny, TDerivedGranny> DerivedGrannyUpdater { get; }
    }
}