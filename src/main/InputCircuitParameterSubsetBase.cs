using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public abstract class InputCircuitParameterSubsetBase : 
        neurULBase,
        IInputCircuitParameterSubset
    {
    }

    public abstract class InputCircuitParameterSubsetBase<T1>(T1? parameter1) : 
        InputCircuitParameterSubsetBase,
        IInputCircuitParameterSubset<T1>
        where T1 : INeuronParameter
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => NetworkHelper.ConvertToNetworks(this.Parameter1);

        public T1? Parameter1 { get; } = parameter1;
    }

    public abstract class InputCircuitParameterSubsetBase<T1, T2>
    (
        T1? parameter1,
        T2? parameter2
    ) :
        InputCircuitParameterSubsetBase<T1>(parameter1),
        IInputCircuitParameterSubset<T1, T2>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => [.. base.GetNetworks(), ..NetworkHelper.ConvertToNetworks(this.Parameter2)];

        public T2? Parameter2 { get; } = parameter2;
    }

    public abstract class InputCircuitParameterSubsetBase<T1, T2, T3>
    (
        T1? parameter1,
        T2? parameter2,
        T3? parameter3
    ) :
        InputCircuitParameterSubsetBase<T1, T2>(parameter1, parameter2),
        IInputCircuitParameterSubset<T1, T2, T3>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => [.. base.GetNetworks(), ..NetworkHelper.ConvertToNetworks(this.Parameter3)];

        public T3? Parameter3 { get; } = parameter3;
    }

    public abstract class InputCircuitParameterSubsetBase<T1, T2, T3, T4>
    (
        T1? parameter1,
        T2? parameter2,
        T3? parameter3,
        T4? parameter4
    ) :
        InputCircuitParameterSubsetBase<T1, T2, T3>(parameter1, parameter2, parameter3),
        IInputCircuitParameterSubset<T1, T2, T3, T4>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
        where T4 : INeuronParameter
    {
        protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => [.. base.GetNetworks(), ..NetworkHelper.ConvertToNetworks(this.Parameter4)];

        public T4? Parameter4 { get; } = parameter4;
    }
}