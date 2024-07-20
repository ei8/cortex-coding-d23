using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Queries
{
    public interface IGrannyQuery
    {
        Task<NeuronQuery> GetQuery(ObtainParameters obtainParameters, IList<IGranny> retrievedGrannies);
    }
}
