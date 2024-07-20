using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Queries
{
    public interface IRetriever : IGrannyQuery
    {
        IGranny RetrieveGranny(Ensemble ensemble, Id23neurULizerOptions options, IEnumerable<IGranny> retrievedGrannies);
    }
}
