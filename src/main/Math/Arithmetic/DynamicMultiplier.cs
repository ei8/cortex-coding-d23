using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    // TODO: Add Adder Circuit to determine current digit
    public partial class DynamicMultiplier
    (
        Multiplier multiplier,
        Adder adder,
        VariableInfo? variableInfo
    ) : 
        CompositeCircuitBase
        <
            DynamicMultiplier,
            Multiplier, 
            Adder
        >
        (
            multiplier,
            adder,
            variableInfo
        ),
        ICompositeCircuitStatic
        <
            DynamicMultiplier,
            Multiplier,
            Adder
        >
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => [.. base.GetNetworks()];

        public static DynamicMultiplier Create
        (
            Multiplier multiplier,
            Adder adder,
            VariableInfo? variableInfo
        ) =>
            new
            (
                multiplier,
                adder,
                variableInfo
            );

        public Multiplier Multiplier => this.Circuit1;

        public Adder Adder => this.Circuit2;
    }
}
