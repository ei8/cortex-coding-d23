﻿using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class GrannyQueryInner<TGranny, TGrannyWriteProcessor, TWriteParameterSet> : IRetriever
        where TGranny : IGranny
        where TGrannyWriteProcessor : IGrannyWriteProcessor<TGranny, TWriteParameterSet>
        where TWriteParameterSet : IWriteParameterSet
    {
        private readonly TGrannyWriteProcessor grannyWriteProcessor;
        private readonly Func<IEnumerable<IGranny>, TWriteParameterSet> writeParametersBuilder;

        public GrannyQueryInner(TGrannyWriteProcessor grannyWriteProcessor, Func<IEnumerable<IGranny>, TWriteParameterSet> writeParametersBuilder)
        {
            AssertionConcern.AssertArgumentNotNull(writeParametersBuilder, nameof(writeParametersBuilder));

            this.grannyWriteProcessor = grannyWriteProcessor;
            this.writeParametersBuilder = writeParametersBuilder;
        }

        public async Task<NeuronQuery> GetQuery(ProcessParameters processParameters, IList<IGranny> retrievedGrannies)
        {
            var gqs = grannyWriteProcessor.GetQueries(processParameters.Options, writeParametersBuilder(retrievedGrannies.AsEnumerable()));
            // process granny queries just like in Extensions.ObtainSync
            var completed = await gqs.Process(processParameters, retrievedGrannies, true);
            // then call GetQuery on last granny query if completed successfully
            return completed ? await gqs.Last().GetQuery(processParameters, retrievedGrannies) : null;
        }

        public IGranny RetrieveGranny(Ensemble ensemble, Id23neurULizerOptions options, IEnumerable<IGranny> retrievedGrannies)
        {
            IGranny result = null;

            if (grannyWriteProcessor.TryParse(ensemble, options, writeParametersBuilder(retrievedGrannies), out TGranny granny))
                result = granny;

            return result;
        }
    }
}
