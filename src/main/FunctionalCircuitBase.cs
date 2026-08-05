using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public abstract class FunctionalCircuitBase<TNeuron> : CircuitBase<FunctionalParameter<TNeuron>, TNeuron>
        where TNeuron : NeuronParameterBase
    {
        protected FunctionalCircuitBase(
            FunctionalParameter<TNeuron> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) : 
        base(
            parameters,
            networks,
            variableInfo
        )
        {
        }
    }
}
