namespace ei8.Cortex.Coding.d23
{
    public class FunctionParameterInfo : ProcedureParameterInfo
    {
        public FunctionParameterInfo(BinaryNeuronInfo[] inputs, BinaryNeuronInfo[] outputs) : base(inputs)
        {
            this.Outputs = outputs;

            this.Network.AddReplaceItems(
                [
                    ..this.Outputs
                ]
            );
        }

        public BinaryNeuronInfo[] Outputs { get; }
    }
}
