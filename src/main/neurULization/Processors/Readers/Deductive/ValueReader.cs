using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ValueReader : IValueReader
    {
        public ValueReader()
        { 
        }

        public IEnumerable<IGrannyQuery> GetQueries(
            Network network, 
            IValueParameterSet parameters
        ) =>
            new[] {
                new GrannyQuery(
                    new NeuronQuery()
                    {
                        Id = new[] { parameters.Value.Id.ToString () }
                    }
                )
            };

        public bool TryParse(Network network, IValueParameterSet parameters, out IValue result)
        {
            result = new Value
            {
                Neuron = parameters.Value
            };

            return result != null;
        }
    }
}
