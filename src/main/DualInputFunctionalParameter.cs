namespace ei8.Cortex.Coding.d23
{
    public class DualInputFunctionalParameter<T> : FunctionalParameter<T>
        where T : NeuronParameterBase
    {
        public DualInputFunctionalParameter(
            T? input1,
            T? input2,
            T? output
        ) : base(
            [
                input1,
                input2
            ],
            [
                output
            ]
        )
        {
            this.Input1 = input1;
            this.Input2 = input2;
            this.Output = output;
        }

        public T? Input1 { get; }
        public T? Input2 { get; }
        public T? Output { get; }
    }
}
