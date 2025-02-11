using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Selectors
{
    public class Relation : ISelector
    {
        private readonly RelativeType type;

        public Relation(RelativeType type)
        {
            this.type = type;
        }

        public IEnumerable<Guid> Evaluate(Network network, IEnumerable<Guid> selection)
        {
            throw new System.NotImplementedException();
        }
    }
}
