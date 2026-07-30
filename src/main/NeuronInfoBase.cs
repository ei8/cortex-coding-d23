namespace ei8.Cortex.Coding.d23
{
    public abstract class NeuronInfoBase : IVariable
    {
        protected Network network;

        protected NeuronInfoBase()
        {
            this.network = new();
        }

        public ReadOnlyNetwork Network => this.network;

        public VariableInfo? VariableInfo { get; protected set; }
    }
}
