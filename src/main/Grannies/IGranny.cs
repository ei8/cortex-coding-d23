using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IGranny
    {
        Neuron Neuron { get; }
    }

    /// <summary>
    /// A specific grandmother may be represented by a specialized ensemble of grandmother or near grandmother cells (Desimone 1991; Gross 1992).
    /// Genealogy of the “Grandmother Cell” - Charles G. Gross
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    /// <typeparam name="TParameterSet"></typeparam>
    public interface IGranny<TSelf, TParameterSet> : IGranny
        where TSelf : IGranny<TSelf, TParameterSet>
        where TParameterSet : IParameterSet
    {
        bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, TParameterSet parameters, out TSelf result);

        IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerOptions options, TParameterSet parameters);

        Task<TSelf> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, TParameterSet parameters);
    }
}
