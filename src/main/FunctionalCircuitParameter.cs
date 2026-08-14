namespace ei8.Cortex.Coding.d23
{
    public class FunctionalCircuitParameter<TInput, TOutput> : ProceduralCircuitParameter<TInput>
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
    {
        public FunctionalCircuitParameter(TInput inputs, TOutput outputs) : base(inputs)
        {
            this.network.AddReplaceItems(
                [
                    (this.Outputs = outputs).Network
                ]
            );
        }

        public TOutput Outputs { get; }
    }
}
