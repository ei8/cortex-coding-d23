using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public interface ICircuit<TParam, TNeuron> : IneurUL 
        where TParam : ParameterBase<TNeuron>, new()
        where TNeuron : NeuronParameterBase
    {
        TParam Parameters { get; }
    }
}
