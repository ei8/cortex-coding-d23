using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal static class UnitExtensions
    {
        internal static IEnumerable<ei8.Cortex.Coding.d23.neurULization.Writers.IUnitParameterSet> GetByTypeId(this IEnumerable<ei8.Cortex.Coding.d23.neurULization.Writers.IUnitParameterSet> units, Guid id, bool isEqual = true) =>
            units.Where(u => isEqual ? u.Type.Id == id : u.Type.Id != id);

        internal static IEnumerable<ei8.Cortex.Coding.d23.neurULization.Readers.IUnitParameterSet> GetByTypeId(this IEnumerable<ei8.Cortex.Coding.d23.neurULization.Readers.IUnitParameterSet> units, Guid id, bool isEqual = true) =>
            units.Where(u => isEqual ? u.Type.Id == id : u.Type.Id != id);
    }
}
