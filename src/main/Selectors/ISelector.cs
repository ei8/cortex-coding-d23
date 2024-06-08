using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Selectors
{
    public interface ISelector
    {
        IEnumerable<Neuron> Evaluate(Ensemble ensemble, IEnumerable<Neuron> selection);
    }
}
