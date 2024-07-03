using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal class BuildAggregateParametersInner<T, TIGranny, TParameterSet, TResult> : ProcessInnerBase<T, TIGranny, TParameterSet, TResult>
            where T : TIGranny, new()
            where TIGranny : IGranny<TIGranny, TParameterSet>
            where TParameterSet : IAggregateParameterSet
    {
        public BuildAggregateParametersInner(
           Func<IGranny, TParameterSet> parametersBuilder,
           Action<TIGranny, TResult> resultUpdater
           ) : base(parametersBuilder, resultUpdater)
        {
        }

        public override IGranny Execute(Ensemble ensemble, IPrimitiveSet primitives, IGranny precedingGranny, TResult tempResult) =>
            throw new NotSupportedException();

        public override async Task<IGranny> ExecuteAsync(Ensemble ensemble, IPrimitiveSet primitives, IGranny precedingGranny, TResult tempResult)
        {
            TIGranny result = await this.granny.ObtainAsync(
                ensemble,
                primitives,
                this.parametersBuilder(precedingGranny)
                );

            this.resultUpdater(result, tempResult);

            return result;
        }
    }
}
