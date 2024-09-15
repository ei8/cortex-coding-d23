using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive;

namespace ei8.Cortex.Coding.d23.neurULization
{
    // TODO: Rename to d23neurULizerInductiveReadOptions
    public class d23neurULizerReadOptions : Id23neurULizerReadOptions
    {
        public d23neurULizerReadOptions(
            PrimitiveSet primitives, 
            string userId, 
            ReadOptions operationOptions, 
            IInstantiatesClass instantiatesClass, 
            IInstanceProcessor instanceProcessor,
            IEnsembleRepository ensembleRepository            
            )
        {
            this.Primitives = primitives;
            this.UserId = userId;
            this.OperationOptions = operationOptions;
            this.InstantiatesClass = instantiatesClass;
            this.InstanceProcessor = instanceProcessor;
            this.EnsembleRepository = ensembleRepository;
        }

        public PrimitiveSet Primitives { get; }

        public string UserId { get; }

        public ReadOptions OperationOptions { get; }

        public IInstantiatesClass InstantiatesClass { get; }

        public IInstanceProcessor InstanceProcessor { get; }

        public IEnsembleRepository EnsembleRepository { get; }
    }
}
