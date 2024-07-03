using ei8.Cortex.Coding.d23.Grannies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23
{
    internal interface IProcessInner<TResult>
    {
        IGranny Execute(Ensemble ensemble, IPrimitiveSet primitives, IGranny precedingGranny, TResult tempResult);
        Task<IGranny> ExecuteAsync(Ensemble ensemble, IPrimitiveSet primitives, IGranny precedingGranny, TResult tempResult);
    }
}
