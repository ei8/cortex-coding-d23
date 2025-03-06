using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    /// <summary>
    /// Represents great granny infos that require a preceding granny.
    /// </summary>
    /// <typeparam name="TGranny"></typeparam>
    /// <typeparam name="TProcessor"></typeparam>
    /// <typeparam name="TParameterSet"></typeparam>
    /// <typeparam name="TAggregate"></typeparam>
    internal interface IDependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate> : ICoreGreatGrannyInfo<TGranny, TProcessor, TAggregate>
        where TGranny : IGranny
        where TProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        Func<IGranny, TParameterSet> ParametersBuilder { get; }
    }
}