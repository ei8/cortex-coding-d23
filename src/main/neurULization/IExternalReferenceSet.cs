namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface IExternalReferenceSet
    {
        Neuron DirectObject { get; set; }

        Neuron Idea { get; set; }

        Neuron Instantiates { get; set; }

        Neuron Simple { get; set; }

        Neuron Subordination { get; set; }

        Neuron Coordination { get; set; }

        Neuron Unit { get; set; }

        Neuron Of { get; set; }

        Neuron Case { get; set; }

        Neuron NominalModifier { get; set; }

        Neuron Has { get; set; }
    }
}
