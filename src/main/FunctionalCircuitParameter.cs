using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public class FunctionalCircuitParameter<TInput, TOutput>
    (
        TInput inputs, 
        TOutput outputs
    ) : 
        ProceduralCircuitParameter<TInput>(inputs)
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => [..base.GetNetworks(), ..NetworkHelper.ConvertToNetworks(this.Outputs)];

        public TOutput Outputs { get; } = outputs;
    }
}
