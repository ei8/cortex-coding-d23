namespace ei8.Cortex.Coding.d23
{
    public class SingleInputFunctionalParameter<T> : FunctionalParameter<T>
        where T : NeuronParameterBase
    {
        public SingleInputFunctionalParameter(
            T? input,
            T? output
        ) : base(
            [
                input,
            ],
            [
                output
            ]
        )
        {
            this.Input = input;
            this.Output = output;
        }

        public T? Input { get; }
        public T? Output { get; }
    }
}
