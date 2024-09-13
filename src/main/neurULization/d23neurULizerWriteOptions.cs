using ei8.Cortex.Coding.d23.neurULization.Processors.Writers;
using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class d23neurULizerWriteOptions : Id23neurULizerWriteOptions
    {
        public d23neurULizerWriteOptions(
            PrimitiveSet primitives, 
            string userId, 
            WriteOptions operationOptions, 
            IInstanceProcessor instanceProcessor, 
            IEnsembleRepository ensembleRepository
        )
        {
            this.Primitives = primitives;
            this.UserId = userId;
            this.OperationOptions = operationOptions;
            this.InstanceProcessor = instanceProcessor;
            this.EnsembleRepository = ensembleRepository;
        }

        public PrimitiveSet Primitives { get; }

        public string UserId { get; }

        public WriteOptions OperationOptions { get; }

        public IInstanceProcessor InstanceProcessor { get; }

        public IEnsembleRepository EnsembleRepository { get; }
    }
}
