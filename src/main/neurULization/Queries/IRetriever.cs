using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Queries
{
    public interface IRetriever : IGrannyQuery
    {
        IGranny RetrieveGranny(Network network, IEnumerable<IGranny> retrievedGrannies);
    }
}
