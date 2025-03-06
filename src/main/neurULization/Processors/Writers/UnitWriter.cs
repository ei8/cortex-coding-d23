using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System;
using System.Linq;

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

            GrannyExtensions.Log($"Building '{typeof(IUnit).Name}'...", 1);

            result.Value = network.AddOrGetIfExists(parameters.Value);
            result.Type = network.AddOrGetIfExists(parameters.Type);
            result.Neuron = network.AddOrGetIfExists(Neuron.CreateTransient(null, null, null));

            new[]
            {
                Tuple.Create(nameof(Unit.Value), result.Value),
                Tuple.Create(nameof(Unit.Type), result.Type)
            }.ToList().ForEach(n =>
            {
                var terminal = Terminal.CreateTransient(result.Neuron.Id, n.Item2.Id);
                GrannyExtensions.Log($"Linking postsynaptic: {terminal.PostsynapticNeuronId} - '{n.Item2.Tag}' ({n.Item1})", 2);
                // add dependency to network
                network.AddReplace(terminal);
            });

            GrannyExtensions.Log($"DONE... Id: {result.Neuron.Id}", 1);

            return result;
        }

        public IGrannyReader<IUnit, IUnitParameterSet> Reader => this.reader;
    }
}
