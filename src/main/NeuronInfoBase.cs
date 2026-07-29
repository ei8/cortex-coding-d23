namespace ei8.Cortex.Coding.d23
{
    public abstract class NeuronInfoBase : IneurUL
    {
        protected Network network;

        protected NeuronInfoBase()
        {
            this.network = new();
        }

        public ReadOnlyNetwork Network => this.network;
    }
}
