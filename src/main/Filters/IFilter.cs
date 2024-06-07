using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Filters
{
    public interface IFilter
    {
        IEnumerable<Neuron> Evaluate(Ensemble ensemble, IEnumerable<Neuron> neurons);
    }
}
