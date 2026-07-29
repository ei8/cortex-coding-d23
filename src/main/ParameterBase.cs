namespace ei8.Cortex.Coding.d23
{
    public class ParameterBase<T> : IneurUL
        where T : NeuronInfoBase
    {
        protected Network network;

        public ParameterBase()
        {
            this.network = new();
        }

        public ReadOnlyNetwork Network => this.network;
    }
}
