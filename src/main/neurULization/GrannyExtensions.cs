using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public static class GrannyExtensions
    {
        internal static void TryParseCore<TGranny>(
            this TGranny granny,
            Network network,
            IEnumerable<Guid> selection,
            LevelParser[] levelParsers,
            Action<Neuron> resultProcessor,
            ref TGranny result,
            bool throwExceptionOnRedundantData = true
        )
            where TGranny : IGranny
        {
            foreach (var levelParser in levelParsers)
                selection = levelParser.Evaluate(network, selection);

            if (selection.Any())
            {
                if (throwExceptionOnRedundantData)
                    AssertionConcern.AssertStateTrue(
                        selection.Count() == 1,
                        $"Redundant items encountered while parsing network: {string.Join(", ", selection)}"
                        );

                if (network.TryGetById(selection.Single(), out Neuron networkResult))
                {
                    resultProcessor(networkResult);
                    result = granny;
                }
            }
        }
    }
}
