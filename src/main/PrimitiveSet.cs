namespace ei8.Cortex.Coding.d23
{
    public class PrimitiveSet : IPrimitiveSet
    {
        public PrimitiveSet()
        {            
        }

        public Neuron DirectObject { get; set; }

        public Neuron Idea { get; set; }

        public Neuron Instantiates { get; set; }

        public Neuron Simple { get; set; }

        public Neuron Subordination { get; set; }

        public Neuron Coordination { get; set; }

        public Neuron Unit { get; set; }

        public Neuron Of { get; set; }

        public Neuron Case { get; set; }
    }
}
