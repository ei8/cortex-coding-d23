using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
{
    /// <summary>
    /// A specific grandmother may be represented by a specialized ensemble of grandmother or near grandmother cells (Desimone 1991; Gross 1992).
    /// Genealogy of the “Grandmother Cell” - Charles G. Gross
    /// </summary>
    public interface IGranny
    {
        Neuron Neuron { get; set; }
    }
}
