using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public static class ExpressionExtensions
    {
        public static IEnumerable<IUnit> GetValueUnitGranniesByTypeId(this IExpression expression, Guid typeId, bool isEqual = true) =>
            expression.Units.Where(u => isEqual ? u.Type.Id == typeId : u.Type.Id != typeId);

        public static bool TryGetValueUnitGrannyByTypeId(this IExpression expression, Guid valueTypeId, out IUnit result)
        {
            var bResult = true;
            var valueUnits = expression.GetValueUnitGranniesByTypeId(valueTypeId);
            if (valueUnits.Count() == 1)
                result = valueUnits.Single();
            else
            {
                if (valueUnits.Count() == 0)
                    Trace.WriteLine($"Value unit with type id '{valueTypeId}' for expression with Id '{expression.Neuron.Id}' was not found.");
                else
                    Trace.WriteLine($"Multiple value units with type id '{valueTypeId}' for expression with Id '{expression.Neuron.Id}' were found.");

                bResult = false;
                result = default;
            }

            return bResult;
        }
    }
}
