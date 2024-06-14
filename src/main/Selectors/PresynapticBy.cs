using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Selectors
{
    public class PresynapticBy : ISelector
    {
        private readonly Func<Neuron, bool> comparer;

        public PresynapticBy(Func<Neuron, bool> comparer)
        {
            this.comparer = comparer;
        }

        public IEnumerable<Neuron> Evaluate(Ensemble ensemble, IEnumerable<Neuron> selection)
        {
            var result = new List<Neuron>();

            // loop through each specified neuron
            foreach (var neuron in selection)
            {
                // loop through each presynaptic
                foreach (var pre in neuron.GetPresynapticNeurons(ensemble))
                {
                    // if exhaustive
                    if (comparer(pre))
                    {
                        // return presynaptic
                        result.Add(pre);
                        break;
                    }
                }
            }

            return result;
        }
    }
}
