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

        internal static bool TryGetGreatGranny<TGreatGranny>(this ILesserGranny lesserGranny, out TGreatGranny result, int depth = 0)
        {
            result = default;
            var bResult = false;

            if (depth >= 0)
            {
                if (depth == 0 && lesserGranny is ILesserGranny<TGreatGranny> lg)
                {
                    bResult = true;
                    result = lg.TypedGreatGranny;
                }
                else if (lesserGranny is ILesserGranny && lesserGranny.GreatGranny is ILesserGranny lg2)
                    bResult = lg2.TryGetGreatGranny(out result, depth - 1);
            }

            return bResult;
        }

        internal static bool TryGetPropertyValue(this IPropertyAssociation propertyAssociation, out Neuron result)
        {
            result = default;

            if (propertyAssociation.TryGetGreatGranny(out IValue pva, 4))
                result = pva.Neuron; 
            else if (propertyAssociation.TryGetGreatGranny(out IInstanceValue piva, 4))
                result = piva.Neuron;

            return result != default;
        }

        internal static bool HasPropertyAssignment(this IPropertyAssociation propertyAssociation, Neuron unit, Neuron property) =>
            propertyAssociation.TryGetGreatGranny(out IPropertyValueAssignment pva) && pva.Expression.HasUnitValue(unit, property) ||
            propertyAssociation.TryGetGreatGranny(out IPropertyInstanceValueAssignment piva) && piva.Expression.HasUnitValue(unit, property);

        internal static bool HasUnitValue(this IExpression expression, Neuron unit, Neuron value) =>
            expression.Units.AsEnumerable().GetValueUnitGranniesByTypeId(unit.Id).SingleOrDefault().Value.Id == value.Id;
    }
}
