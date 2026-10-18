using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Iteration
{
    public partial class DoUntil
    {
        public class WorkingMemoryInfo
        (
            INeuronChunk action,
            IEnumerableChunk<INeuronChunk> counterVariableValues,
            INeuronChunk counterVariable,
            INeuronChunk condition
        ) :
            WorkingMemoryBase
            <
                INeuronChunk,
                IEnumerableChunk<INeuronChunk>,
                INeuronChunk,
                INeuronChunk
            >
            (
                action,
                counterVariableValues,
                counterVariable,
                condition
            )
        {
            public WorkingMemoryInfo
            (
                INeuronChunk action,
                IEnumerableChunk<INeuronChunk> counterVariableValues,
                INeuronChunk counterVariable
            ) : 
                this
                (
                    action,
                    counterVariableValues,
                    counterVariable,
                    counterVariableValues.Items.Last()
                )
            {
            }

            public INeuronChunk Action => this.Content1;
            
            public IEnumerableChunk<INeuronChunk> CounterVariableValues => this.Content2;
            
            public INeuronChunk CounterVariable
            {
                get => this.Content3;
                set => this.Content3 = value;
            }

            public INeuronChunk Condition => this.Content4;
        }
    }
}
