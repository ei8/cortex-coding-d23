using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Queries
{
    public class GrannyQueryInner<TIGranny, TParameterSet> : IRetriever, IReceiver
        where TIGranny : IGranny<TIGranny, TParameterSet>
        where TParameterSet : IParameterSet
    {
        private readonly TIGranny granny;
        private readonly Func<Neuron, TParameterSet> parametersBuilder;
        private Neuron retrievalResult;

        public GrannyQueryInner(TIGranny granny, Func<Neuron, TParameterSet> parametersBuilder)
        {
            AssertionConcern.AssertArgumentNotNull(parametersBuilder, nameof(parametersBuilder));

            this.granny = granny;
            this.parametersBuilder = parametersBuilder;
            this.retrievalResult = null;
        }

        public async Task<NeuronQuery> GetQuery(ObtainParameters obtainParameters)
        {
            var gqs = this.granny.GetQueries(obtainParameters.Options, this.parametersBuilder(this.retrievalResult));
            // process granny queries just like in Extensions.ObtainSync
            var completed = await gqs.Process(obtainParameters, true, this.retrievalResult);
            // then call GetQuery on last granny query if completed successfully
            return completed ? await gqs.Last().GetQuery(obtainParameters) : null;
        }

        public Neuron RetrieveNeuron(Ensemble ensemble, Id23neurULizerOptions options)
        {
            Neuron result = null;

            if (this.granny.TryParse(ensemble, options, this.parametersBuilder(this.retrievalResult), out TIGranny granny))
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
