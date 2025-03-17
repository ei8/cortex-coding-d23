using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ValueWriter : IValueWriter
    {
        private readonly Readers.Deductive.IValueReader reader;
        
        public ValueWriter(Readers.Deductive.IValueReader reader)
        {
            this.reader = reader;
        }

        public IValue Build(Network network, IValueParameterSet parameters) =>
            new Value()
            {
                Neuron = network.AddOrGetIfExists(parameters.Value)
            };

        public IGrannyReader<IValue, IValueParameterSet> Reader => this.reader;
    }
}
