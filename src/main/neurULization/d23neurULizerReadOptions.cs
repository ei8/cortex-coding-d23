using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class d23neurULizerReadOptions : Id23neurULizerReadOptions
    {
        public d23neurULizerReadOptions(IServiceProvider serviceProvider, PrimitiveSet primitives, string userId, ReadOptions operationOptions, IInstantiatesClass instantiatesClass)
        {
            this.ServiceProvider = serviceProvider;
            this.Primitives = primitives;
            this.UserId = userId;
            this.OperationOptions = operationOptions;
            this.InstantiatesClass = instantiatesClass;
        }

        // TODO: remove if possible so parameters are fixed
        public IServiceProvider ServiceProvider { get; }
        public PrimitiveSet Primitives { get; }
        public string UserId { get; }
        public ReadOptions OperationOptions { get; }
        public IInstantiatesClass InstantiatesClass { get; }
    }
}
