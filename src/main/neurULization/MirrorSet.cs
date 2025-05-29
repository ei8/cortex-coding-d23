namespace ei8.Cortex.Coding.d23.neurULization
{
    public class MirrorSet : IMirrorSet
    {
        public MirrorSet()
        {
        }

        public Neuron DirectObject { get; set; }

        public Neuron Instantiates { get; set; }

        public Neuron Unit { get; set; }

        public Neuron Of { get; set; }

        public Neuron Case { get; set; }

        public Neuron NominalModifier { get; set; }

        public Neuron Has { get; set; }

        public Neuron NominalSubject { get; set; }
    }
}
