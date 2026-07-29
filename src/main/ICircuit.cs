namespace ei8.Cortex.Coding.d23
{
    public interface ICircuit<TParam, TNeuron> : IneurUL 
        where TParam : ProceduralParameterInfo<TNeuron>, new()
        where TNeuron : NeuronInfoBase
    {
        TParam Parameters { get; }
    }
}
