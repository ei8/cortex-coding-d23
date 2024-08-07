using ei8.Cortex.Coding.d23.Grannies;
using System;

namespace ei8.Cortex.Coding.d23.neurULization.Writers
{
    internal class GreatGrannyWriteInfo<T, TWriteProcessor, TWriteParameterSet, TDerivedGranny> : IGreatGrannyInfo<TDerivedGranny>
        where T : IGranny
        where TWriteProcessor : IGrannyWriteProcessor<T, TWriteParameterSet>
        where TWriteParameterSet : IWriteParameterSet
    {
        public GreatGrannyWriteInfo(
           TWriteProcessor writeProcessor,
           Func<IGranny, TWriteParameterSet> writeParametersBuilder,
           Action<T, TDerivedGranny> derivedGrannyUpdater
           )
        {
            WriteProcessor = writeProcessor;
            WriteParametersBuilder = writeParametersBuilder;
            DerivedGrannyUpdater = derivedGrannyUpdater;
        }

        public TWriteProcessor WriteProcessor { get; }
        public Func<IGranny, TWriteParameterSet> WriteParametersBuilder { get; }
        public Action<T, TDerivedGranny> DerivedGrannyUpdater { get; }
    }
}