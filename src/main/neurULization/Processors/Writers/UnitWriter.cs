using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class UnitWriter : IUnitWriter
    {
        private readonly Readers.Deductive.IUnitReader reader;

        public UnitWriter(Readers.Deductive.IUnitReader reader)
        {
            this.reader = reader;
        }

        public IUnit Build(Ensemble ensemble, IUnitParameterSet parameters)
        {
            var result = new Unit();
            result.Value = ensemble.AddOrGetIfExists(parameters.Value);
            result.Type = ensemble.AddOrGetIfExists(parameters.Type);
            result.Neuron = ensemble.AddOrGetIfExists(Neuron.CreateTransient(null, null, null));
            // add dependency to ensemble
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Value.Id));
            ensemble.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Type.Id));
            return result;
        }

        public IGrannyReader<IUnit, IUnitParameterSet> Reader => this.reader;
    }
}
