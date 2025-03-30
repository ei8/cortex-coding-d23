using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ValueWriter : IValueWriter
    {
        private readonly Readers.Deductive.IValueReader reader;
        
        public ValueWriter(Readers.Deductive.IValueReader reader)
        {
            this.reader = reader;
        }

        public bool TryBuild(Network network, IValueParameterSet parameters, out IValue result)
        {
            result = null;
            var bResult = false;

            try
            {
                result = new Value()
                {
                    Neuron = network.AddOrGetIfExists(parameters.Value)
                };
                
                bResult = true;
            }
            catch (Exception ex)
            {
                GrannyExtensions.Log($"Error: {ex}");
            }

            return bResult;
        }
        public IGrannyReader<IValue, IValueParameterSet> Reader => this.reader;
    }
}
