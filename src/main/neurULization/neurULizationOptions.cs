namespace ei8.Cortex.Coding.d23.neurULization
{
    public class neurULizationOptions : IneurULizationOptions
    {
        public neurULizationOptions(PrimitiveSet primitives, IEnsembleRepository ensembleRepository, string userId)
        {
            this.Primitives = primitives;
            this.EnsembleRepository = ensembleRepository;
            this.UserId = userId;
        }

        public PrimitiveSet Primitives { get; }

        public IEnsembleRepository EnsembleRepository { get; }

        public string UserId { get; }
    }
}
