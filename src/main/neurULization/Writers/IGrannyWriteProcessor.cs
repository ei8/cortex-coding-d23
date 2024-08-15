using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TGranny"></typeparam>
    /// <typeparam name="TParameterSet"></typeparam>
    public interface IGrannyWriteProcessor<TGranny, TParameterSet> : IGrannyProcessor
        where TGranny : IGranny
        where TParameterSet : IWriteParameterSet
    {
        bool TryParse(Ensemble ensemble, Id23neurULizerWriteOptions options, TParameterSet parameters, out TGranny result);

        IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerWriteOptions options, TParameterSet parameters);

        Task<TGranny> BuildAsync(Ensemble ensemble, Id23neurULizerWriteOptions options, TParameterSet parameters);
    }
}
