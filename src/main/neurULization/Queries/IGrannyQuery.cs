using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Queries
{
    public interface IGrannyQuery
    {
        Task<NeuronQuery> GetQuery(ProcessParameters processParameters, IList<IGranny> retrievedGrannies);
    }
}
