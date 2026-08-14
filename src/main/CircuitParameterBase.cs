namespace ei8.Cortex.Coding.d23
{
    public class CircuitParameterBase : ICircuitParameter
    {
        protected readonly Network network;

        public CircuitParameterBase()
        {
            this.network = new();
        }

        public ReadOnlyNetwork Network => this.network;

        public VariableInfo? VariableInfo { get; protected set; }
    }
}