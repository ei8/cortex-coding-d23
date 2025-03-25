using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ValueReader : IValueReader
    {
        public bool TryParse(Network network, IValueParameterSet parameters, out IValue result)
        {
            var bResult = false;

            if (parameters.Granny != null)
            {
                result = new Value();
                result.Neuron = parameters.Granny;
                bResult = true;
            }
            else
                result = null;

            return bResult;
        }
    }
}
