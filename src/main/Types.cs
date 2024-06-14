using ei8.Cortex.Coding.d23.Grannies;

namespace ei8.Cortex.Coding.d23
{
    public delegate bool TryParseFunc(Ensemble ensemble, IPrimitiveSet primitives, IParameterSet parameters, out IGranny granny);
}
