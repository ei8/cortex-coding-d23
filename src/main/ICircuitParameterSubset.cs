namespace ei8.Cortex.Coding.d23
{
    public interface ICircuitParameterSubset : IneurUL
    {
    }

    public interface IInputCircuitParameterSubset : ICircuitParameterSubset 
    {
    }

    public interface IInputCircuitParameterSubset<T1> : IInputCircuitParameterSubset
        where T1 : INeuronParameter
    {
        T1? Parameter1 { get; }
    }

    public interface IInputCircuitParameterSubset<T1, T2> : IInputCircuitParameterSubset<T1>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
    {
        T2? Parameter2 { get; }
    }

    public interface IInputCircuitParameterSubset<T1, T2, T3> : IInputCircuitParameterSubset<T1, T2>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
    {
        T3? Parameter3 { get; }
    }

    public interface IInputCircuitParameterSubset<T1, T2, T3, T4> : IInputCircuitParameterSubset<T1, T2, T3>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
        where T4 : INeuronParameter
    {
        T4? Parameter4 { get; }
    }

    public interface IInputCircuitParameterSubset<T1, T2, T3, T4, T5> : IInputCircuitParameterSubset<T1, T2, T3, T4>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
        where T4 : INeuronParameter
        where T5 : INeuronParameter
    {
        T5? Parameter5 { get; }
    }

    public interface IOutputCircuitParameterSubset : ICircuitParameterSubset
    {
    }

    public interface IOutputCircuitParameterSubset<T1> : IOutputCircuitParameterSubset
        where T1 : INeuronParameter
    {
        T1? Parameter1 { get; }
    }

    public interface IOutputCircuitParameterSubset<T1, T2> : IOutputCircuitParameterSubset<T1>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
    {
        T2? Parameter2 { get; }
    }

    public interface IOutputCircuitParameterSubset<T1, T2, T3> : IOutputCircuitParameterSubset<T1, T2>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
    {
        T3? Parameter3 { get; }
    }

    public interface IOutputCircuitParameterSubset<T1, T2, T3, T4> : IOutputCircuitParameterSubset<T1, T2, T3>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
        where T4 : INeuronParameter
    {
        T4? Parameter4 { get; }
    }

    public interface IOutputCircuitParameterSubset<T1, T2, T3, T4, T5> : IOutputCircuitParameterSubset<T1, T2, T3, T4>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
        where T4 : INeuronParameter
        where T5 : INeuronParameter
    {
        T5? Parameter5 { get; }
    }
}
