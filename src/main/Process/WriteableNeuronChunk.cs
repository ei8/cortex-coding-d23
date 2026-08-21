namespace ei8.Cortex.Coding.d23.Process
{
    public class WriteableNullableNeuronChunk(Neuron? value) :
        WriteableChunkBase<Neuron?>(value)
    {
        public Neuron? Value
        {
            get => this.Content;
            set => this.Content = value;
        }
    }

    public class WriteableNeuronChunk(Neuron value) :
        WriteableChunkBase<Neuron>(value)
    {
        public Neuron Value 
        { 
            get => this.Content;
            set => this.Content = value;
        }
    }

    public class Writeable2NeuronChunk(Neuron value1, Neuron value2) :
        WriteableChunkBase<Neuron, Neuron>(value1, value2)
    {
        public Neuron Value1
        {
            get => this.Content;
            set => this.Content = value;
        }

        public Neuron Value2
        {
            get => this.Content2;
            set => this.Content2 = value;
        }
    }
}
