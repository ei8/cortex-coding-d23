using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Writers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization
{
    internal static class UnitExtensions
    {
        internal static IEnumerable<Processors.Readers.Deductive.IUnitParameterSet> GetValueUnitParametersByTypeId(this IEnumerable<Processors.Readers.Deductive.IUnitParameterSet> units, Guid id, bool isEqual = true) =>
            units.Where(u => isEqual ? u.Type.Id == id : u.Type.Id != id);

        internal static IEnumerable<Processors.Readers.Inductive.IUnitParameterSet> GetValueUnitParametersByTypeId(this IEnumerable<Processors.Readers.Inductive.IUnitParameterSet> units, Guid id, bool isEqual = true) =>
            units.Where(u => isEqual ? u.Type.Id == id : u.Type.Id != id);

        internal static IEnumerable<IUnit> GetValueUnitGranniesByTypeId(this IEnumerable<IUnit> units, Guid id, bool isEqual = true) =>
            units.Where(u => isEqual ? u.Type.Id == id : u.Type.Id != id);
    }
}
