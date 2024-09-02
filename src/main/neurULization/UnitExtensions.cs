using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal static class UnitExtensions
    {
        internal static IEnumerable<Writers.IUnitParameterSet> GetValueUnitParametersByTypeId(this IEnumerable<Writers.IUnitParameterSet> units, Guid id, bool isEqual = true) =>
            units.Where(u => isEqual ? u.Type.Id == id : u.Type.Id != id);

        internal static IEnumerable<Readers.IUnitParameterSet> GetValueUnitParametersByTypeId(this IEnumerable<Readers.IUnitParameterSet> units, Guid id, bool isEqual = true) =>
            units.Where(u => isEqual ? u.Type.Id == id : u.Type.Id != id);

        internal static IEnumerable<IUnit> GetValueUnitGranniesByTypeId(this IEnumerable<IUnit> units, Guid id, bool isEqual = true) =>
            units.Where(u => isEqual ? u.Type.Id == id : u.Type.Id != id);
    }
}
