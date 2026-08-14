using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public abstract class CircuitBase<TParam> : ICircuit<TParam>, IVariable
        where TParam : CircuitParameterBase
    {
        private readonly Network network;

        protected CircuitBase(TParam parameters, IEnumerable<ReadOnlyNetwork> networks, VariableInfo? variableInfo)
        {
            this.network = new();
            this.network.AddReplaceItems(
                this.Parameters = parameters
            );
            this.network.AddReplaceItems([..networks]);
            this.VariableInfo = variableInfo;
        }

        public ReadOnlyNetwork Network => this.network;

        public TParam Parameters { get; }

        public VariableInfo? VariableInfo { get; }
    }
}
