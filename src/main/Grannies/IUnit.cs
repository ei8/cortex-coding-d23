﻿namespace ei8.Cortex.Coding.d23.Grannies
{
    public interface IUnit : IGranny
    {
        Neuron Value { get; set; }

        Neuron Type { get; set; }
    }
}
