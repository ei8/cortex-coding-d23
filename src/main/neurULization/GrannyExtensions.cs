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
            where TGreatGranny : IGranny
        {
            result = default;
            var bResult = false;

            if (depth >= 0)
            {
                if (depth == 0 && lesserGranny is ILesserGranny<TGreatGranny> lg)
                {
                    bResult = true;
                    result = lg.GreatGranny;
                }
                else if (lesserGranny is ILesserGranny lg2 && lg2.GetGreatGranny() is ILesserGranny lg3)
                    bResult = lg3.TryGetGreatGranny(out result, depth - 1);
            }

            return bResult;
        }

        internal static bool TryGetPropertyValue(this IPropertyAssociation propertyAssociation, out IGranny result)
        {
            result = default;

            if (propertyAssociation.TryGetGreatGranny(out IValue pva, 3))
                result = pva; 
            else if (propertyAssociation.TryGetGreatGranny(out IInstanceValue piva, 3))
                result = piva;

            return result != default;
        }

        internal static string GetValueTag(this IGranny valueGranny, Guid nominalSubjectId)
        {
            string result = null;
            if (valueGranny is IValue)
                result = valueGranny.Neuron.Tag;
            else if (
                valueGranny is IInstanceValue pvg &&
                pvg.Expression.TryGetValueUnitGrannyByTypeId(nominalSubjectId, out IUnit valueUnit)
            )
                result = valueUnit.Value.Tag;

            return result;
        }

        internal static bool HasPropertyAssignment(this IPropertyAssociation propertyAssociation, Neuron unit, Neuron property) =>
            propertyAssociation.TryGetGreatGranny(out IPropertyValueAssignment pva) && pva.Expression.HasUnitValue(unit, property) ||
            propertyAssociation.TryGetGreatGranny(out IPropertyInstanceValueAssignment piva) && piva.Expression.HasUnitValue(unit, property);

        internal static bool HasUnitValue(this IExpression expression, Neuron unit, Neuron value) =>
            expression.TryGetValueUnitGrannyByTypeId(unit.Id, out IUnit result) && result.Value.Id == value.Id;
    }
}
