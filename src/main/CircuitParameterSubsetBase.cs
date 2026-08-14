using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23
{
    public abstract class CircuitParameterSubsetBase : ICircuitParameterSubset
    {
        protected readonly Network network;

        protected CircuitParameterSubsetBase()
        {
            this.network = new();
        }

        protected void AddReplace(INeuronParameter? parameter) =>
            this.AddReplace([parameter]);

        protected void AddReplace(IEnumerable<INeuronParameter?> parameters)
        {
            this.network.AddReplaceItems([.. parameters.WhereNotNull()]);
        }

        public ReadOnlyNetwork Network => this.network;
    }

    public abstract class CircuitParameterSubsetBase<T1>: CircuitParameterSubsetBase
        where T1 : INeuronParameter
    {
        public CircuitParameterSubsetBase(T1? parameter1) => 
            base.AddReplace(this.Parameter1 = parameter1);

        public T1? Parameter1 { get; }
    }

    public abstract class CircuitParameterSubsetBase<T1, T2> : CircuitParameterSubsetBase<T1>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
    {
        public CircuitParameterSubsetBase(T1? parameter1, T2? parameter2) : base(parameter1) => 
            base.AddReplace(this.Parameter2 = parameter2);

        public T2? Parameter2 { get; }
    }

    public abstract class CircuitParameterSubsetBase<T1, T2, T3> : CircuitParameterSubsetBase<T1, T2>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
    {
        public CircuitParameterSubsetBase(T1? parameter1, T2? parameter2, T3? parameter3) : base(parameter1, parameter2) => 
            base.AddReplace(this.Parameter3 = parameter3);

        public T3? Parameter3 { get; }
    }

    public abstract class CircuitParameterSubsetBase<T1, T2, T3, T4> : CircuitParameterSubsetBase<T1, T2, T3>
        where T1 : INeuronParameter
        where T2 : INeuronParameter
        where T3 : INeuronParameter
        where T4 : INeuronParameter
    {
        public CircuitParameterSubsetBase(T1? parameter1, T2? parameter2, T3? parameter3, T4? parameter4) : base(parameter1, parameter2, parameter3) =>
            base.AddReplace(this.Parameter4 = parameter4);

        public T4? Parameter4 { get; }
    }
}