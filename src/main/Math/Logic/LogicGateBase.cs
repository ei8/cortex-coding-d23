using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class LogicGateBase : FunctionalCircuitBase<BinaryNeuronParameter>
    {
        protected LogicGateBase(
            FunctionalParameter<BinaryNeuronParameter> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) : base(
            parameters,
            networks,
            variableInfo
        )
        {
        }
    }
}