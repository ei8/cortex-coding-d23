namespace ei8.Cortex.Coding.d23
{
    public class ProcedureParameterInfo : IneurUL
    {
        public ProcedureParameterInfo(BinaryNeuronInfo[] inputs)
        {
            this.Inputs = inputs;

            this.Network = new();
            this.Network.AddReplaceItems(
                [
                    ..this.Inputs
                ]
            );
        }

        public BinaryNeuronInfo[] Inputs { get; }

        public Network Network { get; }
    }
}
