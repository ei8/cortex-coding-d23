﻿using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TGranny"></typeparam>
    /// <typeparam name="TParameterSet"></typeparam>
    public interface IGrannyReadProcessor<TGranny, TParameterSet> : IGrannyProcessor<TGranny, TParameterSet>
        where TGranny : IGranny
        where TParameterSet : IDeductiveParameterSet
    {
        bool TryParse(Ensemble ensemble, TParameterSet parameters, out TGranny result);

        // TODO: transfer to separate processor (eg. IGrannyQueryProcessor)
        IEnumerable<IGrannyQuery> GetQueries(TParameterSet parameters);
    }
}
