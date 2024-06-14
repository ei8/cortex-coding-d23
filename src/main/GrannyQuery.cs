using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System;

namespace ei8.Cortex.Coding.d23
{
    public class GrannyQuery
    {
        private NeuronQuery neuronQuery;
        private IParameterSet parameters;
        private Func<IParameterSet, NeuronQuery> queryWithParametersBuilder;
        private TryParseFunc tryParser;
        private Func<Neuron, NeuronQuery> queryWithNeuronBuilder;

        public GrannyQuery(NeuronQuery neuronQuery) : this(neuronQuery, null, null, null, null)
        {
        }

        public GrannyQuery(Func<Neuron, NeuronQuery> queryWithNeuronBuilder) : this(null, queryWithNeuronBuilder, null, null, null)
        {
        }

        public GrannyQuery(Func<IParameterSet, NeuronQuery> queryWithParametersBuilder, IParameterSet parameters, TryParseFunc tryParser) :
            this(null, null, queryWithParametersBuilder, parameters, tryParser)
        {
        }

        private GrannyQuery(NeuronQuery neuronQuery, Func<Neuron, NeuronQuery> queryWithNeuronBuilder, Func<IParameterSet, NeuronQuery> queryWithParametersBuilder, IParameterSet parameters, TryParseFunc tryParser)
        {
            this.neuronQuery = neuronQuery;
            this.queryWithNeuronBuilder = queryWithNeuronBuilder;
            this.queryWithParametersBuilder = queryWithParametersBuilder;
            this.parameters = parameters;
            this.tryParser = tryParser;
        }

        public NeuronQuery GetQuery(Neuron previousGrannyNeuron = null)
        {
            NeuronQuery result = null;

            if (this.neuronQuery != null)
                result = this.neuronQuery;
            else if (this.queryWithNeuronBuilder != null)
            {
                AssertionConcern.AssertArgumentNotNull(previousGrannyNeuron, nameof(previousGrannyNeuron));
                result = this.queryWithNeuronBuilder(previousGrannyNeuron);
            }
            else if (this.queryWithParametersBuilder != null)
            {
                result = this.queryWithParametersBuilder(this.parameters);
            }
            else
                throw new InvalidOperationException("Unable to retrieve query as none of the query-related properties have been specified.");

            return result;
        }

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, out IGranny granny) =>
            this.tryParser(ensemble, primitives, this.parameters, out granny);
        
        public bool HasTryParserAndParameters => this.tryParser != null && this.parameters != null;
    }
}
