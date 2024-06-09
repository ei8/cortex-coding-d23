using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    /// <summary>
    /// A specific grandmother may be represented by a specialized ensemble of grandmother or near grandmother cells (Desimone 1991; Gross 1992).
    /// Genealogy of the “Grandmother Cell” - Charles G. Gross
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    /// <typeparam name="TParameterSet"></typeparam>
    public interface IGranny<TSelf, TParameterSet>
        where TSelf : IGranny<TSelf, TParameterSet>
        where TParameterSet : IParameterSet
    {
        bool TryParse(Ensemble ensemble, ICoreSet coreSet, TParameterSet parameterSet, out TSelf result);

        IEnumerable<NeuronQuery> GetQueries(ICoreSet coreSet, TParameterSet parameterSet);

        Task<TSelf> BuildAsync(Ensemble ensemble, ICoreSet coreSet, TParameterSet parameterSet);

        Neuron Neuron { get; }
    }
}
