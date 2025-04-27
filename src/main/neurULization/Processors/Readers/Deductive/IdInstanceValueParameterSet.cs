using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class IdInstanceValueParameterSet : InstanceValueParameterSet, IIdInstanceValueParameterSet
    {
        public IdInstanceValueParameterSet(
            Neuron value,
            Neuron @class,
            ValueMatchBy valueMatchBy,
            Guid id
        ) : base(value, @class, valueMatchBy)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}
