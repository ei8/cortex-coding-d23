using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class neurULizerOptions : Id23neurULizerOptions
    {
        public neurULizerOptions(IServiceProvider serviceProvider, PrimitiveSet primitives, string userId)
        {
            this.ServiceProvider = serviceProvider;
            this.Primitives = primitives;
            this.UserId = userId;
        }

        public IServiceProvider ServiceProvider { get; }
        public PrimitiveSet Primitives { get; }
        public string UserId { get; }
    }
}
