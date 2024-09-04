using ei8.Cortex.Coding.d23.Grannies;
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
        // TODO: Id23neurULizerWriteOptions should this be here?
        bool TryParse(Ensemble ensemble, Id23neurULizerWriteOptions options, TParameterSet parameters, out TGranny result);

        IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerWriteOptions options, TParameterSet parameters);
    }
}
