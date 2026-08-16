using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public abstract class neurULBase : IneurUL
    {
        protected abstract IEnumerable<ReadOnlyNetwork> GetNetworks();

        public ReadOnlyNetwork Network => this.GetNetworks().FromNetworks();
    }
}
