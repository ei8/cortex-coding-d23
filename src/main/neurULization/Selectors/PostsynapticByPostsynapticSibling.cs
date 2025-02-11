using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Selectors
{
    public class PostsynapticByPostsynapticSibling : ISelector
    {
        private readonly IEnumerable<Guid> siblingNeuronIds;

        public PostsynapticByPostsynapticSibling(params Guid[] siblingNeuronIds)
        {
            this.siblingNeuronIds = siblingNeuronIds;
        }

        public IEnumerable<Guid> Evaluate(Network network, IEnumerable<Guid> selection)
        {
            var result = new List<Guid>();

            // loop through each specified neuron
            foreach (var neuronId in selection)
            {
                // get postsynaptics of current neuron
                var posts = network.GetPostsynapticNeurons(neuronId);

                // if current neuron has postsynaptics equal in number to 1 + specified postsynaptic siblings 
                if (posts.Count() == siblingNeuronIds.Count() + 1)
                {
                    // get postsynaptic except specified siblings
                    var diff = posts.Select(n => n.Id).Except(siblingNeuronIds);

                    if (diff.Count() == 1)
                        // return difference
                        result.Add(diff.Single());
                }
            }

            return result;
        }
    }
}
