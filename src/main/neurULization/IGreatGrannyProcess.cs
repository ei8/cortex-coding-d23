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

        bool TryExecute(
            IGreatGrannyInfo<TAggregate> greatGrannyInfo, 
            Network network, 
            TAggregate aggregate, 
            IParameterSet parameters,
            out IGranny result
        );

        void UpdateAggregate(
            IGreatGrannyInfo<TAggregate> greatGrannyInfo,
            IGranny precedingGranny,
            TAggregate aggregate
        );
    }
}
