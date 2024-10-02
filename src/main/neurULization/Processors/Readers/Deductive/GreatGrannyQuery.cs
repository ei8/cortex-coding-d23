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
    public class GreatGrannyQuery<TGranny, TGrannyReader, TParameterSet> : IRetriever
        where TGranny : IGranny
        where TGrannyReader : IGrannyReader<TGranny, TParameterSet>
        where TParameterSet : IDeductiveParameterSet
    {
        private readonly TGrannyReader grannyReader;
        private readonly Func<IEnumerable<IGranny>, TParameterSet> parametersBuilder;

        public GreatGrannyQuery(
            TGrannyReader grannyReader,
            Func<IEnumerable<IGranny>, TParameterSet> parametersBuilder,
            bool requiresPrecedingGrannyQueryResult = true
            )
        {
            AssertionConcern.AssertArgumentNotNull(grannyReader, nameof(GreatGrannyQuery<TGranny, TGrannyReader, TParameterSet>.grannyReader));
            AssertionConcern.AssertArgumentNotNull(parametersBuilder, nameof(parametersBuilder));

            this.grannyReader = grannyReader;
            this.parametersBuilder = parametersBuilder;
            this.RequiresPrecedingGrannyQueryResult = requiresPrecedingGrannyQueryResult;
        }

        public bool RequiresPrecedingGrannyQueryResult { get; }

        public async Task<NeuronQuery> GetQuery(IEnsembleRepository ensembleRepository, Ensemble ensemble, IList<IGranny> retrievedGrannies, string userId)
        {
            var gqs = grannyReader.GetQueries(parametersBuilder(retrievedGrannies.AsEnumerable()));
            // process granny queries
            var completed = await gqs.Process(
                ensembleRepository, 
                ensemble, 
                retrievedGrannies, 
                userId,
                true
            );
            // then call GetQuery on last granny query if completed successfully
            return completed ? 
                await gqs.Last().GetQuery(
                    ensembleRepository, 
                    ensemble, 
                    retrievedGrannies, 
                    userId
                ) : 
                null;
        }

        public IGranny RetrieveGranny(Ensemble ensemble, IEnumerable<IGranny> retrievedGrannies)
        {
            IGranny result = null;

            if (grannyReader.TryParse(ensemble, parametersBuilder(retrievedGrannies), out TGranny granny))
                result = granny;

            return result;
        }
    }
}
