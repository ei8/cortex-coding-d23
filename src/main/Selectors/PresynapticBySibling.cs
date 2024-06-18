using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Selectors
{
    public class PresynapticBySibling : ISelector
    {
        private readonly bool exhaustive;
        private readonly IEnumerable<Neuron> siblingNeurons;

        public PresynapticBySibling(params Neuron[] siblingNeurons) : this(true, siblingNeurons)
        {
        }

        public PresynapticBySibling(bool exhaustive, params Neuron[] siblingNeurons)
        {
            this.exhaustive = exhaustive;
            this.siblingNeurons = siblingNeurons;
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
                    if (exhaustive)
                    {
                        var preTerminals = ensemble.GetTerminals(pre.Id);
                        // if presynaptic has only current neuron + siblings as postsynaptic 
                        if (preTerminals.Count() == siblingNeurons.Count() + 1 &&
                            // and postsynaptics of presynaptic match the current neuron and the siblings
                            preTerminals.Select(t => t.PostsynapticNeuronId).HasSameElementsAs(
                                siblingNeurons.Select(sn => sn.Id).Concat(new[] { neuronId }))
                            )
                        {
                            // return presynaptic
                            result.Add(pre.Id);
                        }
                    }
                }
            }

            return result;
        }
    }
}
