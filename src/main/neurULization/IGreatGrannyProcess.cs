using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public interface IGreatGrannyProcess<TAggregate>
        where TAggregate : IGranny
    {
        bool TryGetParameters(
            IGranny precedingGranny,
            IGreatGrannyInfo<TAggregate> greatGrannyInfo,
            out IParameterSet parameters
        );

        IGranny Execute(
            IGreatGrannyInfo<TAggregate> greatGrannyInfo, 
            Ensemble ensemble, 
            TAggregate aggregate, 
            IParameterSet parameters
        );

        void UpdateAggregate(
            IGreatGrannyInfo<TAggregate> greatGrannyInfo,
            IGranny precedingGranny,
            TAggregate aggregate
        );
    }
}
