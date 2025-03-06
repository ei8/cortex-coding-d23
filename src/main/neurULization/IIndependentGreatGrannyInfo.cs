using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    /// <summary>
    /// Represents great granny infos that do not require preceding grannies.
    /// </summary>
    /// <typeparam name="TGranny"></typeparam>
    /// <typeparam name="TProcessor"></typeparam>
    /// <typeparam name="TParameterSet"></typeparam>
    /// <typeparam name="TAggregate"></typeparam>
    internal interface IIndependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate> : ICoreGreatGrannyInfo<TGranny, TProcessor, TAggregate>
        where TGranny : IGranny
        where TProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        Func<TParameterSet> ParametersBuilder { get; }
    }
}