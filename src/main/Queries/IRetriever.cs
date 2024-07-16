using ei8.Cortex.Coding.d23.neurULization;

namespace ei8.Cortex.Coding.d23.Queries
{
    public interface IRetriever : IGrannyQuery
    {
        Neuron RetrieveNeuron(Ensemble ensemble, Id23neurULizerOptions options);
    }
}
