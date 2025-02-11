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

        public IUnit Build(Network network, IUnitParameterSet parameters)
        {
            var result = new Unit();
            result.Value = network.AddOrGetIfExists(parameters.Value);
            result.Type = network.AddOrGetIfExists(parameters.Type);
            result.Neuron = network.AddOrGetIfExists(Neuron.CreateTransient(null, null, null));
            // add dependency to network
            network.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Value.Id));
            network.AddReplace(Terminal.CreateTransient(result.Neuron.Id, result.Type.Id));
            return result;
        }

        public IGrannyReader<IUnit, IUnitParameterSet> Reader => this.reader;
    }
}
