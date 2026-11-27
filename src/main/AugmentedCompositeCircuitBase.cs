using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public abstract class AugmentedCompositeCircuitBase<T, TAug, T1, T2>
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
        where T : IAugmentedCompositeCircuitStatic<T, TAug, T1, T2>
        where TAug : IAugmentation
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

    }
}