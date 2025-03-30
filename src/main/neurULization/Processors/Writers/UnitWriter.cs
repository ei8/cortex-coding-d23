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

        public bool TryBuild(Network network, IUnitParameterSet parameters, out IUnit result)
        {
            result = null;
            var bResult = false;

            try
            {
                GrannyExtensions.Log($"Building '{typeof(IUnit).Name}'...", 1);

                var tempResult = new Unit()
                {
                    Value = network.AddOrGetIfExists(parameters.Value),
                    Type = network.AddOrGetIfExists(parameters.Type),
                    Neuron = network.AddOrGetIfExists(Neuron.CreateTransient(null, null, null))
                };

                new[]
                {
                    Tuple.Create(nameof(Unit.Value), tempResult.Value),
                    Tuple.Create(nameof(Unit.Type), tempResult.Type)
                }.ToList().ForEach(n =>
                {
                    var terminal = Terminal.CreateTransient(tempResult.Neuron.Id, n.Item2.Id);
                    GrannyExtensions.Log($"Linking postsynaptic: {terminal.PostsynapticNeuronId} - '{n.Item2.Tag}' ({n.Item1})", 2);
                    // add dependency to network
                    network.AddReplace(terminal);
                });

                GrannyExtensions.Log($"DONE... Id: {tempResult.Neuron.Id}", 1);

                result = tempResult;
                bResult = true;
            }
            catch (Exception ex)
            {
                GrannyExtensions.Log($"Error: {ex}");
            }

            return bResult;
        }

        public IGrannyReader<IUnit, IUnitParameterSet> Reader => this.reader;
    }
}
