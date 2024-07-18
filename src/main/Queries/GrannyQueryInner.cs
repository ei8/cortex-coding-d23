using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Queries
{
    public class GrannyQueryInner<TGranny, TGrannyProcessor, TParameterSet> : IRetriever, IReceiver
        where TGranny : IGranny
        where TGrannyProcessor : IGrannyProcessor<TGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        private readonly TGrannyProcessor grannyProcessor;
        private readonly Func<Neuron, TParameterSet> parametersBuilder;
        private Neuron retrievalResult;

        public GrannyQueryInner(TGrannyProcessor grannyProcessor, Func<Neuron, TParameterSet> parametersBuilder)
        {
            AssertionConcern.AssertArgumentNotNull(parametersBuilder, nameof(parametersBuilder));

            this.grannyProcessor = grannyProcessor;
            this.parametersBuilder = parametersBuilder;
            this.retrievalResult = null;
        }

        public async Task<NeuronQuery> GetQuery(ObtainParameters obtainParameters)
        {
            var gqs = this.grannyProcessor.GetQueries(obtainParameters.Options, this.parametersBuilder(this.retrievalResult));
            // process granny queries just like in Extensions.ObtainSync
            var completed = await gqs.Process(obtainParameters, true, this.retrievalResult);
            // then call GetQuery on last granny query if completed successfully
            return completed ? await gqs.Last().GetQuery(obtainParameters) : null;
        }

        public Neuron RetrieveNeuron(Ensemble ensemble, Id23neurULizerOptions options)
        {
            Neuron result = null;

            if (this.grannyProcessor.TryParse(ensemble, options, this.parametersBuilder(this.retrievalResult), out TGranny granny))
                result = granny.Neuron;

            return result;
        }

        public void SetPrecedingRetrievalResult(Neuron value)
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            this.retrievalResult = value;
        }
    }
}
