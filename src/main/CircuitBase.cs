using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public abstract class CircuitBase<TParam, TNeuron> : ICircuit<TParam, TNeuron>, IVariable
        where TParam : ParameterBase<TNeuron>, new()
        where TNeuron : NeuronParameterBase
    {
        private Network network;

        protected CircuitBase()
        {
            this.network = new();
            this.Parameters = new();
        }

        protected virtual void Initialize(
            TParam parameters,
            IEnumerable<ReadOnlyNetwork> networks
        )
        {
            this.network.AddReplaceItems(
                this.Parameters = parameters
            );
            this.network.AddReplaceItems([..networks]);
        }

        public ReadOnlyNetwork Network => this.network;

        public TParam Parameters { get; protected set; }

        public VariableInfo? VariableInfo { get; protected set; }
    }
}
