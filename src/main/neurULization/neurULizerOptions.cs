using System;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class neurULizerOptions : Id23neurULizerOptions
    {
        public neurULizerOptions(IServiceProvider serviceProvider, PrimitiveSet primitives, string userId, WriteMode writeMode)
        {
            this.ServiceProvider = serviceProvider;
            this.Primitives = primitives;
            this.UserId = userId;
            this.WriteMode = writeMode;
        }

        public IServiceProvider ServiceProvider { get; }
        public PrimitiveSet Primitives { get; }
        public string UserId { get; }
        public WriteMode WriteMode { get; }
    }
}
