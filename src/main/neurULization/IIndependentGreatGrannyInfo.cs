﻿using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal interface IIndependentGreatGrannyInfo<TGranny, TProcessor, TParameterSet, TAggregate> : ICoreGreatGrannyInfo<TGranny, TProcessor, TAggregate>
        where TGranny : IGranny
        where TProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        Func<TParameterSet> ParametersBuilder { get; }
    }
}