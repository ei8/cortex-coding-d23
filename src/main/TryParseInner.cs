using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal class TryParseInner<T, TIGranny, TParameterSet, TResult> : ProcessInnerBase<T, TIGranny, TParameterSet, TResult>
            where T : TIGranny, new()
            where TIGranny : IGranny<TIGranny, TParameterSet>
            where TParameterSet : IParameterSet
    {
        public TryParseInner(
           Func<IGranny, TParameterSet> parametersBuilder,
           Action<TIGranny, TResult> resultUpdater
           ) : base(parametersBuilder, resultUpdater)
        {
        }

        public override IGranny Execute(Ensemble ensemble, IPrimitiveSet primitives, IGranny precedingGranny, TResult tempResult)
        {
            IGranny result = null;

            if (this.granny.TryParse(
                ensemble,
                primitives,
                this.parametersBuilder(precedingGranny),
                out TIGranny gr)
                )
            {
                this.resultUpdater(gr, tempResult);
                result = gr;
            }

            return result;
        }

        public override Task<IGranny> ExecuteAsync(Ensemble ensemble, IPrimitiveSet primitives, IGranny precedingGranny, TResult tempResult) =>
            throw new NotSupportedException();
    }
}
