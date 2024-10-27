using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    internal interface IInductiveIndependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate> : ICoreGreatGrannyInfo<TGranny, TProcessor, TAggregate>, IInductiveGreatGrannyInfo<TAggregate>
        where TGranny : IGranny
        where TProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        Func<TParameterSet> ParametersBuilder { get; }
    }
}