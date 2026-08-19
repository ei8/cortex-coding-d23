using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23
{
    public abstract class CompositeCircuitBase<T1, T2>
    ( 
        T1 circuit1,
        T2 circuit2,
        VariableInfo? variableInfo
    ) : 
        CircuitBase(variableInfo)
        where T1 : ICircuit
        where T2 : ICircuit
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            NetworkHelper.ConvertToNetworks(this.Circuit1, Circuit2);

        public T1 Circuit1 { get; } = circuit1;

        public T2 Circuit2 { get; } = circuit2;

        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            T1 circuit1,
            T2 circuit2,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
            where T : ICreatableCompositeCircuit<T, T1, T2>
        {
            bool bResult = false;
            result = default;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                result = T.Create(
                    circuit1,
                    circuit2,
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }
    }
}
