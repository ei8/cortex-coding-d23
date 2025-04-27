using neurUL.Common.Domain.Model;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class InstanceValueExpressionParameterSet : InstanceValueParameterSet, IInstanceValueExpressionParameterSet
    {
        public InstanceValueExpressionParameterSet(
            Neuron value,
            Neuron @class,
            ValueMatchBy valueMatchBy
        ) : base(value, @class, valueMatchBy)
        {
        }
    }
}
