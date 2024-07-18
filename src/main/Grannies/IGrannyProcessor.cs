using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TGranny"></typeparam>
    /// <typeparam name="TParameterSet"></typeparam>
    public interface IGrannyProcessor<TGranny, TParameterSet>
        where TGranny : IGranny
        where TParameterSet : IParameterSet
    {
        bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, TParameterSet parameters, out TGranny result);

        IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerOptions options, TParameterSet parameters);

        Task<TGranny> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, TParameterSet parameters);
    }
}
