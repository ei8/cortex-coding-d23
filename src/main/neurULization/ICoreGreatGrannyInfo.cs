using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal interface ICoreGreatGrannyInfo<TGranny, TProcessor, TAggregate> : IGreatGrannyInfo<TAggregate>
        where TGranny : IGranny
    {
        TProcessor Processor { get; }
        Action<TGranny, TAggregate> AggregateUpdater { get; }
    }
}