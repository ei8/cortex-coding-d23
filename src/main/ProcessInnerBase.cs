using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal abstract class ProcessInnerBase<T, TIGranny, TParameterSet, TResult> : IProcessInner<TResult>
            where T : TIGranny, new()
            where TIGranny : IGranny<TIGranny, TParameterSet>
            where TParameterSet : IParameterSet
    {
        protected T granny;
        protected Func<IGranny, TParameterSet> parametersBuilder;
        protected Action<TIGranny, TResult> resultUpdater;

        public ProcessInnerBase(
           Func<IGranny, TParameterSet> parametersBuilder,
           Action<TIGranny, TResult> resultUpdater
           )
        {
            this.granny = new T();
            this.parametersBuilder = parametersBuilder;
            this.resultUpdater = resultUpdater;
        }

        public abstract IGranny Execute(Ensemble ensemble, IPrimitiveSet primitives, IGranny precedingGranny, TResult tempResult);

        public abstract Task<IGranny> ExecuteAsync(Ensemble ensemble, IPrimitiveSet primitives, IGranny precedingGranny, TResult tempResult);
    }
}