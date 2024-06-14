using ei8.Cortex.Coding.d23.Selectors;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public class Dependent : IDependent
    {
        public Dependent()
        {
        }

        public async Task<IDependent> BuildAsync(Ensemble ensemble, IPrimitiveSet primitives, IDependentParameterSet parameters)
        {
            var result = new Dependent();
            result.Value = ensemble.Obtain(parameters.Value);
            result.Type = ensemble.Obtain(parameters.Type);
            result.Neuron = ensemble.Obtain(Neuron.CreateTransient(null, null, null));
            // add dependency to ensemble
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Value.Id));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Type.Id));
            return result;
        }

        public IEnumerable<GrannyQuery> GetQueries(IPrimitiveSet primitives, IDependentParameterSet parameters) =>
            new[] {
                new GrannyQuery(
                    new NeuronQuery()
                    {
                        Postsynaptic = new[] {
                            parameters.Value.Id.ToString(),
                            parameters.Type.Id.ToString()
                        },
                        DirectionValues = DirectionValues.Outbound,
                        Depth = 1
                    }
                )
            };

        public bool TryParse(Ensemble ensemble, IPrimitiveSet primitives, IDependentParameterSet parameters, out IDependent result)
        {
            result = null;

            var tempResult = new Dependent();
            tempResult.Value = parameters.Value;
            tempResult.Type = parameters.Type;
            
            this.TryParseCore(
                parameters,
                ensemble,
                tempResult,
                new[] { tempResult.Value },
                new[] { new LevelParser(new PresynapticBySibling(tempResult.Type)) },
                (n) => tempResult.Neuron = n,
                ref result
                );

            return result != null;
        }

        public Neuron Value { get; private set; }

        public Neuron Type { get; private set; }

        public Neuron Neuron { get; private set; }
    }
}
