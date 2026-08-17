using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public class ProceduralCircuitParameter<TInput>(TInput inputs) : 
        CircuitParameterBase(),
        IProceduralCircuitParameter
        where TInput : IInputCircuitParameterSubset
    {
        public TInput Inputs { get; } = inputs;

        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => NetworkHelper.ConvertToNetworks(this.Inputs);
    }
}
