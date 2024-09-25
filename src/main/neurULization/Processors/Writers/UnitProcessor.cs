using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class UnitProcessor : IUnitProcessor
    {
        private readonly Readers.Deductive.IUnitProcessor readProcessor;

        public UnitProcessor(Readers.Deductive.IUnitProcessor readProcessor)
        {
            this.readProcessor = readProcessor;
        }

        public async Task<IUnit> BuildAsync(Ensemble ensemble, IUnitParameterSet parameters)
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

        public IGrannyReadProcessor<IUnit, IUnitParameterSet> ReadProcessor => this.readProcessor;
    }
}
