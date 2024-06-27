using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System;

namespace ei8.Cortex.Coding.d23
{
    public class GrannyQuery : IGrannyQuery
    {
        private NeuronQuery neuronQuery;

        public GrannyQuery(NeuronQuery neuronQuery) 
        {
            AssertionConcern.AssertArgumentNotNull(neuronQuery, nameof(neuronQuery));
            this.neuronQuery = neuronQuery;
        }

        public NeuronQuery GetQuery()
        {
            return this.neuronQuery;
        }
    }

    public class GrannyQueryBuilder : IReceiver
    {
        private Func<Neuron, NeuronQuery> queryWithNeuronBuilder;
        private Neuron retrievalResult;

        public GrannyQueryBuilder(Func<Neuron, NeuronQuery> queryWithNeuronBuilder) 
        {
            AssertionConcern.AssertArgumentNotNull(queryWithNeuronBuilder, nameof(queryWithNeuronBuilder));
            this.queryWithNeuronBuilder = queryWithNeuronBuilder;
            this.retrievalResult = null;
        }

        public NeuronQuery GetQuery()
        {
            AssertionConcern.AssertStateTrue(this.retrievalResult != null, "RetrievalResult is required to invoke GetQuery.");
            return this.queryWithNeuronBuilder(this.retrievalResult);
        }

        public void SetRetrievalResult(Neuron value)
        {
            AssertionConcern.AssertArgumentNotNull(value, nameof(value));
            this.retrievalResult = value;
        }
    }

    public class GrannyQueryParser<T> : IRetriever
        where T : IParameterSet
    {
        private T parameters;
        private Func<T, NeuronQuery> queryWithParametersBuilder;
        private TryParseFunc<T> tryParser;

        public GrannyQueryParser(T parameters, Func<T, NeuronQuery> queryWithParametersBuilder, TryParseFunc<T> tryParser)
        {
            AssertionConcern.AssertArgumentNotNull(parameters, nameof(parameters));
            AssertionConcern.AssertArgumentNotNull(queryWithParametersBuilder, nameof(queryWithParametersBuilder));
            AssertionConcern.AssertArgumentNotNull(tryParser, nameof(tryParser));

            this.parameters = parameters;
            this.queryWithParametersBuilder = queryWithParametersBuilder;
            this.tryParser = tryParser;
        }

        public NeuronQuery GetQuery()
        {
            return this.queryWithParametersBuilder(this.parameters);
        }

        public Neuron RetrieveNeuron(Ensemble ensemble, IPrimitiveSet primitives)
        {
            Neuron result = null;

            if (this.tryParser(ensemble, primitives, this.parameters, out IGranny granny))
                result = granny.Neuron;

            return result;
        }
    }

    public interface IGrannyQuery
    {
        NeuronQuery GetQuery();
    }

    public interface IReceiver : IGrannyQuery
    {
        void SetRetrievalResult(Neuron value);
    }

    public interface IRetriever : IGrannyQuery
    {
        Neuron RetrieveNeuron(Ensemble ensemble, IPrimitiveSet primitives);
    }
}
