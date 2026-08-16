namespace ei8.Cortex.Coding.d23
{
    public class InputCircuitParameterSubset<T1>(T1? input1) : 
        CircuitParameterSubsetBase<T1>(input1), 
        IInputCircuitParameterSubset
        where T1 : INeuronParameter
    {
    }

    public class InputCircuitParameterSubset<T1, T2>(T1? input1, T2? input2) : 
        CircuitParameterSubsetBase<T1, T2>(input1, input2), 
        IInputCircuitParameterSubset
        where T1 : INeuronParameter
        where T2 : INeuronParameter
    {
    }

    public class InputCircuitParameterSubset<T1, T2, T3>(T1? input1, T2? input2, T3? input3) : 
        CircuitParameterSubsetBase<T1, T2, T3>(input1, input2, input3), 
        IInputCircuitParameterSubset
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
    {
    }

    public class InputCircuitParameterSubset<T1, T2, T3, T4>(T1? input1, T2? input2, T3? input3, T4? input4) :
        CircuitParameterSubsetBase<T1, T2, T3, T4>(input1, input2, input3, input4), 
        IInputCircuitParameterSubset
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
        where T4 : INeuronParameter
    {
    }
}
