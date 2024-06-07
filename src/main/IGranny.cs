using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    public interface IGranny<TSelf, TParameterSet>
        where TSelf : IGranny<TSelf, TParameterSet>
        where TParameterSet : IParameterSet
    {
        bool TryParse(Ensemble ensemble, ICoreSet coreSet, TParameterSet parameterSet, out Neuron result);

        IEnumerable<NeuronQuery> GetQueries(ICoreSet coreSet, TParameterSet parameterSet);

        Task<Neuron> BuildAsync(Ensemble ensemble, ICoreSet coreSet, TParameterSet parameterSet);
    }
}
