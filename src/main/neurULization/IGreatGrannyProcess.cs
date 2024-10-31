using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface IGreatGrannyProcess<TAggregate>
        where TAggregate : IGranny
    {
        IGranny Execute(
            IGreatGrannyInfo<TAggregate> greatGrannyInfo,
            Ensemble ensemble,
            IGranny precedingGranny,
            TAggregate aggregate
        );

        void UpdateAggregate(
            IGreatGrannyInfo<TAggregate> greatGrannyInfo,
            IGranny precedingGranny,
            TAggregate aggregate
        );
    }
}
