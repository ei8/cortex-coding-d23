namespace ei8.Cortex.Coding.d23.neurULization
{
    public class neurULizationOptions
    {
        public neurULizationOptions(ICoreSet coreSet, IEnsembleRepository ensembleRepository, string userId)
        {
            this.CoreSet = coreSet;
            this.EnsembleRepository = ensembleRepository;
            this.UserId = userId;
        }

        public ICoreSet CoreSet { get; }

        public IEnsembleRepository EnsembleRepository { get; }

        public string UserId { get; }
    }
}
