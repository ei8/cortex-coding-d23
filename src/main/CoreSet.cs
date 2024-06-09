namespace ei8.Cortex.Coding.d23
{
    public class CoreSet : ICoreSet
    {
        public CoreSet()
        {            
        }

        public Neuron DirectObject { get; set; }

        public Neuron InstantiatesUnit { get; set; }

        public Neuron Subordination { get; set; }

        public Neuron Unit { get; set; }
    }
}
