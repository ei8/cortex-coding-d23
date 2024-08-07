using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Queries
{
    public interface IRetriever : IGrannyQuery
    {
        IGranny RetrieveGranny(Ensemble ensemble, Id23neurULizerOptions options, IEnumerable<IGranny> retrievedGrannies);
    }
}
