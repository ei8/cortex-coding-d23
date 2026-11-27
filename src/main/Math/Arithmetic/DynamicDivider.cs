using ei8.Cortex.Coding.d23.Math.Comparison;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    // TODO: Add Adder Circuit to determine current digit
    public partial class DynamicDivider
    (
        Divider divider,
        Comparator comparator,
        Multiplier multiplier,
        Subtractor subtractor,
        VariableInfo? variableInfo
    ) :
        CompositeCircuitBase
        <
            DynamicDivider,
            Divider,
            Comparator,
            Multiplier,
            Subtractor
        >
        (
            divider,
            comparator,
            multiplier,
            subtractor,
            variableInfo
        ),
        ICompositeCircuitStatic
        <
            DynamicDivider,
            Divider,
            Comparator,
            Multiplier,
            Subtractor
        >
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => [.. base.GetNetworks()];

        public static DynamicDivider Create
        (
            Divider divider,
            Comparator comparator,
            Multiplier multiplier,
            Subtractor subtractor,
            VariableInfo? variableInfo
        ) =>
            new
            (
                divider,
                comparator,
                multiplier,
                subtractor,
                variableInfo
            );

        public Divider Divider => this.Circuit1;

        public Comparator Comparator => this.Circuit2;

        public Multiplier Multiplier => this.Circuit3;

        public Subtractor Subtractor => this.Circuit4;
    }
}
