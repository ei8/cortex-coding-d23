namespace ei8.Cortex.Coding.d23
{
    public abstract class NeuronParameterBase : IVariable
    {
        protected Network network;

        protected NeuronParameterBase()
        {
            this.network = new();
        }

        public int Strength { get; }

        public NeurotransmitterEffect Effect { get; }

        public ReadOnlyNetwork Network => this.network;

        public VariableInfo? VariableInfo { get; protected set; }
    }
}
