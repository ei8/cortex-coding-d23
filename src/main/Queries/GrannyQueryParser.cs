using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Queries
{
    public class GrannyQueryParser<T> : IRetriever, IReceiver
        where T : IParameterSet
    {
        private Func<Neuron, T> parametersBuilder;
        private Func<T, IEnumerable<IGrannyQuery>> queryWithParametersBuilder;
        private TryParseFunc<T> tryParser;
        private Neuron retrievalResult;

        public GrannyQueryParser(Func<Neuron, T> parametersBuilder, Func<T, IEnumerable<IGrannyQuery>> queryWithParametersBuilder, TryParseFunc<T> tryParser)
        {
            AssertionConcern.AssertArgumentNotNull(parametersBuilder, nameof(parametersBuilder));
            AssertionConcern.AssertArgumentNotNull(queryWithParametersBuilder, nameof(queryWithParametersBuilder));
            AssertionConcern.AssertArgumentNotNull(tryParser, nameof(tryParser));

            this.parametersBuilder = parametersBuilder;
            this.queryWithParametersBuilder = queryWithParametersBuilder;
            this.tryParser = tryParser;
            this.retrievalResult = null;
        }

        public async Task<NeuronQuery> GetQuery(ObtainParameters obtainParameters)
        {
            var gqs = queryWithParametersBuilder(this.parametersBuilder(this.retrievalResult));
            // process granny queries just like in Extensions.ObtainSync
            var completed = await gqs.Process(obtainParameters, true, this.retrievalResult);
            // then call GetQuery on last granny query if completed successfully
            return completed ? await gqs.Last().GetQuery(obtainParameters) : null;
        }

        public Neuron RetrieveNeuron(Ensemble ensemble, IPrimitiveSet primitives)
        {
            Neuron result = null;

            if (this.tryParser(ensemble, primitives, this.parametersBuilder(this.retrievalResult), out IGranny granny))
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
