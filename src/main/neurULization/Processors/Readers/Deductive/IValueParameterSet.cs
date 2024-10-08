﻿namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public interface IValueParameterSet : IDeductiveParameterSet
    {
        Neuron Value { get; }

        Neuron Class { get; }

        ValueMatchBy ValueMatchBy { get; }
    }
}
