using System;
using System.Diagnostics.CodeAnalysis;

namespace ei8.Cortex.Coding.d23
{
    public delegate bool TryFunc<TResult>
    (
        [NotNullWhen(true)]
        out TResult result
    );

    public delegate bool TryFunc<T, TResult>
    (
        T value,
        [NotNullWhen(true)]
        out TResult result
    );
}
