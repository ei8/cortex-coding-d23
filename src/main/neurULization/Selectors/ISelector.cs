using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.neurULization.Selectors
{
    public interface ISelector
    {
        IEnumerable<Guid> Evaluate(Ensemble ensemble, IEnumerable<Guid> selection);
    }
}
