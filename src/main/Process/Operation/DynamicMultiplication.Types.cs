namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class DynamicMultiplication
    {
        public class WorkingMemoryInfo
        (
            EnumerableChunk multiplicandValues,
            EnumerableChunk multiplierValues,
            EnumerableChunk productValues,
            NestedListChunk multiplierProducts,
            ListChunk product
        ) :
            WorkingMemoryBase
            <
                EnumerableChunk,
                EnumerableChunk,
                EnumerableChunk,
                NestedListChunk,
                ListChunk
            >
            (
                multiplicandValues,
                multiplierValues,
                productValues,
                multiplierProducts,
                product
            )
        {
            public WorkingMemoryInfo
            (
                EnumerableChunk multiplicandValues,
                EnumerableChunk multiplierValues,
                EnumerableChunk productValues
            ) :
                this
                (
                    multiplicandValues,
                    multiplierValues,
                    productValues,
                    new(),
                    new()
                )
            {
            }

            public EnumerableChunk MultiplicandValues => this.Chunk1;

            public EnumerableChunk MultiplierValues => this.Chunk2;

            public EnumerableChunk ProductValues => this.Chunk3;

            public NestedListChunk MultiplierProducts => this.Chunk4;

            public ListChunk Product => this.Chunk5;
        }
    }
}