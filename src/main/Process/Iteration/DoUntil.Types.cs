using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Iteration
{
    public partial class DoUntil
    {
        public class WorkingMemoryInfo
        (
            NeuronChunk action,
            EnumerableChunk<NeuronChunk> counterVariableValues,
            NeuronChunk counterVariable,
            NeuronChunk condition
        ) :
            IWorkingMemory
        {
            public WorkingMemoryInfo
            (
                NeuronChunk action,
                EnumerableChunk<NeuronChunk> counterVariableValues,
                NeuronChunk counterVariable
            ) : 
                this
                (
                    action,
                    counterVariableValues,
                    counterVariable,
                    counterVariableValues.Content.Last()
                )
            {
            }

            public NeuronChunk Action => action;
            public EnumerableChunk<NeuronChunk> CounterVariableValues => counterVariableValues;
            public NeuronChunk CounterVariable { get; set; } = counterVariable;
            public NeuronChunk Condition => condition;
        }
    }
}
