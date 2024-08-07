using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Selectors
{
    public class PresynapticBy : ISelector
    {
        private readonly Func<Neuron, bool> comparer;

        public PresynapticBy(Func<Neuron, bool> comparer)
        {
            this.comparer = comparer;
        }

        public IEnumerable<Guid> Evaluate(Ensemble ensemble, IEnumerable<Guid> selection)
        {
            var result = new List<Guid>();

            // loop through each specified neuron
            foreach (var neuronId in selection)
            {
                // loop through each presynaptic
                foreach (var pre in ensemble.GetPresynapticNeurons(neuronId))
                {
                    // if exhaustive
                    if (comparer(pre))
                    {
                        // return presynaptic
                        result.Add(pre.Id);
                        break;
                    }
                }
            }

            return result;
        }
    }
}
