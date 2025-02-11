using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Selectors
{
    public class PresynapticByPostsynapticSibling : ISelector
    {
        private readonly IEnumerable<Guid> siblingNeuronIds;

        public PresynapticByPostsynapticSibling(params Guid[] siblingNeuronIds)
        {
            this.siblingNeuronIds = siblingNeuronIds;
        }

        public IEnumerable<Guid> Evaluate(Network network, IEnumerable<Guid> selection)
        {
            var result = new List<Guid>();

            // loop through each specified neuron
            foreach (var neuronId in selection)
            {
                // loop through each presynaptic
                foreach (var pre in network.GetPresynapticNeurons(neuronId))
                {
                    var preTerminals = network.GetTerminals(pre.Id);
                    // if presynaptic has only current neuron + siblings as postsynaptic 
                    if (preTerminals.Count() == siblingNeuronIds.Count() + 1 &&
                        // and postsynaptics of presynaptic match the current neuron and the siblings
                        preTerminals.Select(t => t.PostsynapticNeuronId).HasSameElementsAs(
                            siblingNeuronIds.Concat(new[] { neuronId }))
                        )
                    {
                        // return presynaptic
                        result.Add(pre.Id);
                    }
                }
            }

            return result;
        }
    }
}
