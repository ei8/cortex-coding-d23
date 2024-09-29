using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class GreatGrannyQuery<TGranny, TGrannyWriteProcessor, TWriteParameterSet> : IRetriever
        where TGranny : IGranny
        where TGrannyWriteProcessor : IGrannyReadProcessor<TGranny, TWriteParameterSet>
        where TWriteParameterSet : IDeductiveParameterSet
    {
        private readonly TGrannyWriteProcessor grannyWriteProcessor;
        private readonly Func<IEnumerable<IGranny>, TWriteParameterSet> writeParametersBuilder;

        public GreatGrannyQuery(
            TGrannyWriteProcessor grannyWriteProcessor,
            Func<IEnumerable<IGranny>, TWriteParameterSet> writeParametersBuilder,
            bool requiresPrecedingGrannyQueryResult = true
            )
        {
            AssertionConcern.AssertArgumentNotNull(grannyWriteProcessor, nameof(grannyWriteProcessor));
            AssertionConcern.AssertArgumentNotNull(writeParametersBuilder, nameof(writeParametersBuilder));

            this.grannyWriteProcessor = grannyWriteProcessor;
            this.writeParametersBuilder = writeParametersBuilder;
            this.RequiresPrecedingGrannyQueryResult = requiresPrecedingGrannyQueryResult;
        }

        public bool RequiresPrecedingGrannyQueryResult { get; }

        public async Task<NeuronQuery> GetQuery(IEnsembleRepository ensembleRepository, Ensemble ensemble, IList<IGranny> retrievedGrannies, string userId, string cortexLibraryOutBaseUrl, int queryResultLimit)
        {
            var gqs = grannyWriteProcessor.GetQueries(writeParametersBuilder(retrievedGrannies.AsEnumerable()));
            // process granny queries
            var completed = await gqs.Process(
                ensembleRepository, 
                ensemble, 
                retrievedGrannies, 
                userId,  
                cortexLibraryOutBaseUrl,
                queryResultLimit,
                true
            );
            // then call GetQuery on last granny query if completed successfully
            return completed ? 
                await gqs.Last().GetQuery(
                    ensembleRepository, 
                    ensemble, 
                    retrievedGrannies, 
                    userId,
                    cortexLibraryOutBaseUrl,
                    queryResultLimit
                ) : 
                null;
        }

        public IGranny RetrieveGranny(Ensemble ensemble, IEnumerable<IGranny> retrievedGrannies)
        {
            IGranny result = null;

            if (grannyWriteProcessor.TryParse(ensemble, writeParametersBuilder(retrievedGrannies), out TGranny granny))
                result = granny;

            return result;
        }
    }
}
