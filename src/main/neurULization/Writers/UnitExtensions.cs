using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    internal static class UnitExtensions
    {
        internal static IEnumerable<IUnitParameterSet> GetByTypeId(this IEnumerable<IUnitParameterSet> units, Guid id, bool isEqual = true) =>
            units.Where(u => isEqual ? u.Type.Id == id : u.Type.Id != id);
    }
}
