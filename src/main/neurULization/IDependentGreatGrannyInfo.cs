using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal interface IDependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate> : ICoreGreatGrannyInfo<TGranny, TProcessor, TAggregate>
        where TGranny : IGranny
        where TProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        Func<IGranny, TParameterSet> ParametersBuilder { get; }
    }
}