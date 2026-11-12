using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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
            Multiplier, 
            Adder
        >
        (
            multiplier,
            adder,
            variableInfo
        )
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

        public static bool TryCreate
        (
            [NotNullWhen(true)] out DynamicMultiplier? result,
            Multiplier multiplier,
            Adder adder,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
        {
            bool bResult = false;
            result = default;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                result = DynamicMultiplier.Create
                (
                    multiplier,
                    adder,
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }

        public Multiplier Multiplier => this.Circuit1;

        public Adder Adder => this.Circuit2;
    }
}
