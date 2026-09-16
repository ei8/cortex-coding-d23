namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    // TODO: So OperationBase can implement IOperation
    // 1. transfer static abstract functions to IOperationStatic or 
    // 2. Create base interface for IOperation and implement from here,  eg. see LogicGateBase, ILogicGate
    // TOOD: look for similar patterns where this update is applicable
    public interface IGroupedOperation
    <
        T, 
        TParam, 
        TInterneuron
    > : 
        IOperation
        <
            T,
            TParam,
            TInterneuron
        >
        where T : ICircuit<TParam, TInterneuron>
        where TParam : IFunctionalCircuitParameter
        where TInterneuron : ICircuitInterneuronSet
    {
        static abstract TParam GetDefaultParameters(
            BinaryNeuronParameter? precedingValue,
            int exponent
        );

        static abstract TInterneuron CreateInterneurons(
            TParam parameters,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        );
    }
}
