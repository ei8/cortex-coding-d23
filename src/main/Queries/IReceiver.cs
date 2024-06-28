using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23.Queries
{
    public interface IReceiver : IGrannyQuery
    {
        void SetRetrievalResult(Neuron value);
    }
}
