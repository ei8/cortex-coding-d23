﻿using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    public class UnitProcessor : IUnitProcessor
    {
        public async Task<IUnit> BuildAsync(Ensemble ensemble, Id23neurULizerWriteOptions options, IUnitParameterSet parameters)
        {
            var result = new Unit();
            result.Value = ensemble.Obtain(parameters.Value);
            result.Type = ensemble.Obtain(parameters.Type);
            result.Neuron = ensemble.Obtain(Neuron.CreateTransient(null, null, null));
            // add dependency to ensemble
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Value.Id));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Type.Id));
            return result;
        }

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerWriteOptions options, IUnitParameterSet parameters) =>
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

        public bool TryParse(Ensemble ensemble, Id23neurULizerWriteOptions options, IUnitParameterSet parameters, out IUnit result)
        {
            result = null;

            var tempResult = new Unit();
            tempResult.Value = parameters.Value;
            tempResult.Type = parameters.Type;

            tempResult.TryParseCore(
                ensemble,
                new[] { tempResult.Value.Id },
                new[] { new LevelParser(new PresynapticByPostsynapticSibling(tempResult.Type.Id)) },
                (n) => tempResult.Neuron = n,
                ref result
                );

            return result != null;
        }
    }
}
