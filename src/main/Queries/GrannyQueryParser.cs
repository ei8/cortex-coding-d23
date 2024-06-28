using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Library.Common;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Queries
{
    public class GrannyQueryParser<T> : IRetriever
        where T : IParameterSet
    {
        private T parameters;
        private Func<T, IEnumerable<IGrannyQuery>> queryWithParametersBuilder;
        private TryParseFunc<T> tryParser;

        public GrannyQueryParser(T parameters, Func<T, IEnumerable<IGrannyQuery>> queryWithParametersBuilder, TryParseFunc<T> tryParser)
        {
            AssertionConcern.AssertArgumentNotNull(parameters, nameof(parameters));
            AssertionConcern.AssertArgumentNotNull(queryWithParametersBuilder, nameof(queryWithParametersBuilder));
            AssertionConcern.AssertArgumentNotNull(tryParser, nameof(tryParser));

            this.parameters = parameters;
            this.queryWithParametersBuilder = queryWithParametersBuilder;
            this.tryParser = tryParser;
        }

        public async Task<NeuronQuery> GetQuery(ObtainParameters obtainParameters)
        {
            var gqs = queryWithParametersBuilder(parameters);
            // process granny queries just like in Extensions.ObtainSync
            await gqs.Process(obtainParameters, true);
            // then call GetQuery on last granny query
            return await gqs.Last().GetQuery(obtainParameters);
        }

        public Neuron RetrieveNeuron(Ensemble ensemble, IPrimitiveSet primitives)
        {
            Neuron result = null;

            if (tryParser(ensemble, primitives, parameters, out IGranny granny))
                result = granny.Neuron;

            return result;
        }
    }
}
