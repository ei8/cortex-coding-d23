using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Queries
{
    public class GrannyQueryInner<TGranny, TGrannyProcessor, TParameterSet> : IRetriever
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        private readonly TGrannyProcessor grannyProcessor;
        private readonly Func<IEnumerable<IGranny>, TParameterSet> parametersBuilder;

        public GrannyQueryInner(TGrannyProcessor grannyProcessor, Func<IEnumerable<IGranny>, TParameterSet> parametersBuilder)
        {
            AssertionConcern.AssertArgumentNotNull(parametersBuilder, nameof(parametersBuilder));

            this.grannyProcessor = grannyProcessor;
            this.parametersBuilder = parametersBuilder;
        }

        public async Task<NeuronQuery> GetQuery(ObtainParameters obtainParameters, IList<IGranny> retrievedGrannies)
        {
            var gqs = this.grannyProcessor.GetQueries(obtainParameters.Options, this.parametersBuilder(retrievedGrannies.AsEnumerable()));
            // process granny queries just like in Extensions.ObtainSync
            var completed = await gqs.Process(obtainParameters, retrievedGrannies, true);
            // then call GetQuery on last granny query if completed successfully
            return completed ? await gqs.Last().GetQuery(obtainParameters, retrievedGrannies) : null;
        }

        public IGranny RetrieveGranny(Ensemble ensemble, Id23neurULizerOptions options, IEnumerable<IGranny> retrievedGrannies)
        {
            IGranny result = null;

            if (this.grannyProcessor.TryParse(ensemble, options, this.parametersBuilder(retrievedGrannies), out TGranny granny))
                result = granny;

            return result;
        }
    }
}
