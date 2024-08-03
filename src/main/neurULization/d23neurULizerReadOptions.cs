using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class d23neurULizerReadOptions : Id23neurULizerReadOptions
    {
        public d23neurULizerReadOptions(IServiceProvider serviceProvider, PrimitiveSet primitives, string userId, ReadOptions operationOptions)
        {
            this.ServiceProvider = serviceProvider;
            this.Primitives = primitives;
            this.UserId = userId;
            this.OperationOptions = operationOptions;
        }

        public IServiceProvider ServiceProvider { get; }
        public PrimitiveSet Primitives { get; }
        public string UserId { get; }
        public ReadOptions OperationOptions { get; }
    }
}
