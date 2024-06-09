﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.d23
{
    public interface ICoreSet
    {
        Neuron DirectObject { get; }

        Neuron InstantiatesUnit { get; }

        Neuron Subordination { get; }

        Neuron Unit { get; }
    }
}
