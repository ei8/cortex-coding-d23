namespace ei8.Cortex.Coding.d23.Queries
{
    public interface IRetriever : IGrannyQuery
    {
        Neuron RetrieveNeuron(Ensemble ensemble, IPrimitiveSet primitives);
    }
}
