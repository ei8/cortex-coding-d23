using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal static class NetworkExtensions
    {
        public static bool TryGetByTag(this Network network, string tag, out IEnumerable<Neuron> result) 
        {
            bool result2 = false;
            result = null;
            var matches = network.GetItems<Neuron>().Where(n => n.Tag == tag);

            if (matches.Any())
            {
                result = matches;
                result2 = true;
            }

            return result2;
        }
    }
}
