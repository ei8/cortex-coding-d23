namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface IMirrorSet
    {
        Neuron DirectObject { get; set; }

        Neuron Idea { get; set; }

        Neuron Instantiates { get; set; }

        Neuron Unit { get; set; }

        Neuron Of { get; set; }

        Neuron Case { get; set; }

        Neuron NominalModifier { get; set; }

        Neuron Has { get; set; }

        Neuron NominalSubject { get; set; }
    }
}
