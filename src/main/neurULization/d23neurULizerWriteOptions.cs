using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class d23neurULizerWriteOptions : Id23neurULizerWriteOptions
    {
        public d23neurULizerWriteOptions(IServiceProvider serviceProvider, PrimitiveSet primitives, string userId, WriteOptions operationOptions)
        {
            this.ServiceProvider = serviceProvider;
            this.Primitives = primitives;
            this.UserId = userId;
            this.OperationOptions = operationOptions;
        }

        public IServiceProvider ServiceProvider { get; }
        public PrimitiveSet Primitives { get; }
        public string UserId { get; }
        public WriteOptions OperationOptions { get; }
    }
}
