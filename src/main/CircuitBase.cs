using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public abstract class CircuitBase
    (
        VariableInfo? variableInfo
    ) : 
        neurULBase,
        ICircuit,
        IVariable
    {
        public VariableInfo? VariableInfo { get; } = variableInfo;
    }
    public abstract class CircuitBase<TParam, TInterneuron>
    (
        TParam parameters,
        TInterneuron interneurons,
        VariableInfo? variableInfo
    ) : 
        CircuitBase(variableInfo),
        ICircuit<TParam, TInterneuron>
        where TParam : ICircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
        [
            this.Parameters.Network,
            this.Interneurons.Network
        ];

        public TParam Parameters { get; } = parameters;

        public TInterneuron Interneurons { get; } = interneurons;
    }
}
