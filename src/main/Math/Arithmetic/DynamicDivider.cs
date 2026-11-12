using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
     public partial class DynamicDivider
     (
        Divider divider,
        Subtractor subtractor,
        VariableInfo? variableInfo
    ) :
        CompositeCircuitBase
        <
            Divider,
            Subtractor
        >
        (
            divider,
            subtractor,
            variableInfo
        )
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => [.. base.GetNetworks()];

        public static DynamicDivider Create
        (
            Divider divider,
            Subtractor subtractor,
            VariableInfo? variableInfo
        ) =>
            new
            (
                divider,
                subtractor,
                variableInfo
            );

        public static bool TryCreate
        (
            [NotNullWhen(true)] out DynamicDivider? result,
            Divider divider,
            Subtractor subtractor,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
        {
            bool bResult = false;
            result = default;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                result = DynamicDivider.Create
                (
                    divider,
                    subtractor,
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }

        public Divider Divider => this.Circuit1;

        public Subtractor Adder => this.Circuit2;
    }
}
