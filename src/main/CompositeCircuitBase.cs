using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23
{
    public abstract class CompositeCircuitBase<T, T1, T2>
    ( 
        T1 circuit1,
        T2 circuit2,
        VariableInfo? variableInfo
    ) : 
        CircuitBase(variableInfo),
        ICompositeCircuit
        <
            T1,
            T2
        >
        where T : ICompositeCircuitStatic<T, T1, T2>
        where T1 : ICircuit
        where T2 : ICircuit
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            PerfNetworkHelper.ConvertToNetworks
            (
                this.Circuit1, 
                this.Circuit2
            );

        public T1 Circuit1 => circuit1;

        public T2 Circuit2 => circuit2;

        public static bool TryCreate(
            [NotNullWhen(true)] out T? result,
            T1 circuit1,
            T2 circuit2,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
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

    public abstract class CompositeCircuitBase<T, T1, T2, T3>
    (
        T1 circuit1,
        T2 circuit2,
        T3 circuit3,
        VariableInfo? variableInfo
    ) :
        CircuitBase(variableInfo),
        ICompositeCircuit
        <
            T1,
            T2,
            T3
        >
        where T : ICompositeCircuitStatic<T, T1, T2, T3>
        where T1 : ICircuit
        where T2 : ICircuit
        where T3 : ICircuit
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            PerfNetworkHelper.ConvertToNetworks
            (
                this.Circuit1, 
                this.Circuit2,
                this.Circuit3
            );

        public T1 Circuit1 => circuit1;

        public T2 Circuit2 => circuit2;

        public T3 Circuit3 => circuit3;

        public static bool TryCreate(
            [NotNullWhen(true)] 
            out T? result,
            T1 circuit1,
            T2 circuit2,
            T3 circuit3,
            [CallerArgumentExpression(nameof(result))] 
            string parameterExpression = ""
        )
        {
            bool bResult = false;
            result = default;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                result = T.Create(
                    circuit1,
                    circuit2,
                    circuit3,
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }
    }

    public abstract class CompositeCircuitBase<T, T1, T2, T3, T4>
    (
        T1 circuit1,
        T2 circuit2,
        T3 circuit3,
        T4 circuit4,
        VariableInfo? variableInfo
    ) :
        CircuitBase(variableInfo),
        ICompositeCircuit
        <
            T1,
            T2,
            T3,
            T4
        >
        where T : ICompositeCircuitStatic<T, T1, T2, T3, T4>
        where T1 : ICircuit
        where T2 : ICircuit
        where T3 : ICircuit
        where T4 : ICircuit
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
            PerfNetworkHelper.ConvertToNetworks
            (
                this.Circuit1,
                this.Circuit2,
                this.Circuit3,
                this.Circuit4
            );

        public T1 Circuit1 => circuit1;

        public T2 Circuit2 => circuit2;

        public T3 Circuit3 => circuit3;
        
        public T4 Circuit4 => circuit4;

        public static bool TryCreate(
            [NotNullWhen(true)]
            out T? result,
            T1 circuit1,
            T2 circuit2,
            T3 circuit3,
            T4 circuit4,
            [CallerArgumentExpression(nameof(result))]
            string parameterExpression = ""
        )
        {
            bool bResult = false;
            result = default;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                result = T.Create(
                    circuit1,
                    circuit2,
                    circuit3,
                    circuit4,
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }
    }
}
