using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Queries
{
    public class GrannyQueryBuilder : IGrannyQuery
    {
        private Func<IEnumerable<IGranny>, NeuronQuery> queryWithNeuronBuilder;

        public GrannyQueryBuilder(Func<IEnumerable<IGranny>, NeuronQuery> queryWithNeuronBuilder)
        {
            AssertionConcern.AssertArgumentNotNull(queryWithNeuronBuilder, nameof(queryWithNeuronBuilder));
            this.queryWithNeuronBuilder = queryWithNeuronBuilder;
        }

        public Task<NeuronQuery> GetQuery(ProcessParameters processParameters, IList<IGranny> retrievedGrannies)
        {
            return Task.FromResult(queryWithNeuronBuilder(retrievedGrannies.AsEnumerable()));
        }
    }
}
