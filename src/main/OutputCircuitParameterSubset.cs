namespace ei8.Cortex.Coding.d23
{
    public class OutputCircuitParameterSubset<T1>(T1? output1)
        : CircuitParameterSubsetBase<T1>(output1), IOutputCircuitParameterSubset
        where T1 : INeuronParameter
    {
    }

    public class OutputCircuitParameterSubset<T1, T2>(T1? output1, T2? output2)
        : CircuitParameterSubsetBase<T1, T2>(output1, output2), IOutputCircuitParameterSubset
        where T1 : INeuronParameter
        where T2 : INeuronParameter
    {
    }

    public class OutputCircuitParameterSubset<T1, T2, T3>(T1? output1, T2? output2, T3? output3)
        : CircuitParameterSubsetBase<T1, T2, T3>(output1, output2, output3), IOutputCircuitParameterSubset
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
    {
    }

    public class OutputCircuitParameterSubset<T1, T2, T3, T4>(T1? output1, T2? output2, T3? output3, T4? output4)
        : CircuitParameterSubsetBase<T1, T2, T3, T4>(output1, output2, output3, output4), IOutputCircuitParameterSubset
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
        where T4 : INeuronParameter
    {
    }
}
