using ei8.Cortex.Coding.d23.Math.TruthTables;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    // DEL:
    public abstract class GroupedOperationBase
    <
        T,
        TParam, 
        TInput,
        TOutput,
        TInterneuron,
        TInterneuronTagger
    >
    (
        TParam parameters,
        TInterneuron interneurons,
        VariableInfo? variableInfo
    ) : 
        TruthTableBase
        <
            T,
            TParam,
            TInput,
            TOutput,
            TInterneuron,
            TInterneuronTagger
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        IGroupedOperation
        <
            TParam,
            TInterneuron
        >
        where T :
            ITruthTableStatic
            <
                T,
                TParam,
                TInput,
                TOutput,
                TInterneuron,
                TInterneuronTagger
            >
        where TParam : IFunctionalCircuitParameter<TInput, TOutput>
        where TInput : IInputCircuitParameterSubset
        where TOutput : IOutputCircuitParameterSubset
        where TInterneuron : ICircuitInterneuronSet
        where TInterneuronTagger : IKeyedInterneuronTagger
    {
    }
}
