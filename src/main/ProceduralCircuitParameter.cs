namespace ei8.Cortex.Coding.d23
{
    public class ProceduralCircuitParameter<TInput> : CircuitParameterBase
        where TInput : IInputCircuitParameterSubset
    {
        public ProceduralCircuitParameter(TInput inputs) : base()
        {
            this.network.AddReplaceItems(
                [
                    (this.Inputs = inputs).Network
                ]
            );
        }

        public TInput Inputs { get; }
    }
}
