using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23
{
    public delegate bool TryParseFunc<T>(Ensemble ensemble, IPrimitiveSet primitives, T parameters, out IGranny granny) where T : IParameterSet;
}
