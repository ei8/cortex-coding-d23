using ei8.Cortex.Coding.d23.Process.Iteration;
using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    // TODO: public class Addition : IProcess
    //{
    //    public class WorkingMemoryKeys(
    //        ReadOnlyChunk addend1Digits,
    //        ReadOnlyChunk addend2Digits,
    //        WriteableChunk currentDigit,
    //        WriteableChunk digitSums,
    //        ReadOnlyChunk digitCount
    //    )
    //    {
    //        // TODO: how to reuse single adder for whole operation?
    //        // create Do Until that fires output for each (step / max) of (DigitAddend1Values / DigitAddend2Values)?
    //        DigitAddend1Values,
    //        DigitAddend2Values,
    //        DigitSums, // store results from each digit
    //        DigitCount // eg. Do Until (Condition) - eg. Digit7
    //    }

    //    private DoUntil doUntil;

    //    public Addition()
    //    {
    //        this.doUntil = new DoUntil();
    //    }

    //    public IEnumerable<Neuron> GetCurrent()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void HandleFire(Neuron target, ReadOnlyNetwork network)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Initialize(IWorkingMemory workingMemory, Action completionCallback)
    //    {
    //        // TODO: this.doUntil.Initialize(
    //        //    WorkingMemory.Create>(
    //        //        ReadableKeyedChunk.Create(DoUntil.WorkingMemoryKeys.
    //        //    )
    //    }
    //}
}
