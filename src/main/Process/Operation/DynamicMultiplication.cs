using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class DynamicMultiplication :
        FiniteCompositeProcessBase
        <
            DynamicMultiplication.WorkingMemoryInfo,
            DynamicAddition,
            Action<DynamicMultiplication, IEnumerable<Neuron>>
        >
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Addition.WorkingMemoryInfo additionWorkingMemory;

        public DynamicMultiplication
        (
            DynamicMultiplication.WorkingMemoryInfo workingMemory,
            Addition.WorkingMemoryInfo additionWorkingMemory,
            Action<DynamicMultiplication, IEnumerable<Neuron>> completionCallback
        ) :
            base
            (
                workingMemory,
                completionCallback
            )
        {
            this.additionWorkingMemory = additionWorkingMemory;
        }

        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];

            if(this.DynamicAddition == null)
                result.AddRange(this.WorkingMemory.GetCurrent().Select(c => c.Neuron));
            else
                result.AddRange(this.DynamicAddition.GetCurrent());

            return result;
        }

        public override bool HandleFire(Neuron targetNeuron)
        {
            ArgumentNullException.ThrowIfNull(this.WorkingMemory.Multiplier.Value);

            var result = false;

            if(this.DynamicAddition == null)
            {
                int currentMultiplierDigitIndex = -1;

                if
                (
                    this.WorkingMemory.Product.DigitValues.Items.Any(c => c.Neuron == targetNeuron) &&
                    this.WorkingMemory.Multiplicand.HasCurrentDigitChanged() &&
                    this.WorkingMemory.Multiplier.CurrentDigit != null &&
                    (currentMultiplierDigitIndex = this.WorkingMemory.Multiplier.Value.Items.ToList().IndexOf(this.WorkingMemory.Multiplier.CurrentDigit)) >= 0
                )
                {
                    result = true;

                    this.WorkingMemory.Multiplicand.UpdateTracking();
                    
                    if (this.WorkingMemory.MultiplierProducts.Lists.Count < currentMultiplierDigitIndex + 1)
                        this.WorkingMemory.MultiplierProducts.Lists.Add(new ListChunk<INeuronChunk>());

                    this.WorkingMemory.MultiplierProducts.Lists[currentMultiplierDigitIndex].Items.Add(new NeuronChunk(targetNeuron));

                    DynamicMultiplication.logger.Info
                    (
                        new LogMessageGenerator
                        (
                            () => $"Added to MultiplierProduct(s): {targetNeuron.Tag}, " +
                                $"{ 
                                    string.Join
                                    (
                                        string.Empty, 
                                        this.WorkingMemory.MultiplierProducts
                                            .Lists[currentMultiplierDigitIndex]
                                            .Items.Reverse().Select(d => d.Neuron.Tag.Last())
                                    )
                                }"
                        )
                    );

                    if(this.WorkingMemory.Multiplicand.CurrentDigit == null)
                    {
                        this.WorkingMemory.Multiplier.UpdateTracking();
                        if(this.WorkingMemory.Multiplier.CurrentDigit != null)
                        {
                            this.WorkingMemory.Multiplicand.ResetTracking();
                        }
                        else if (this.WorkingMemory.MultiplierProducts.Lists.Count > 0)
                        {
                            if (this.WorkingMemory.MultiplierProducts.Lists.Count == 1)
                            {
                                foreach (var mp in this.WorkingMemory.MultiplierProducts.Lists[0].Items)
                                    this.WorkingMemory.Product.Value.Add(mp);
                                this.Complete();
                            }
                            else
                            {
                                this.Process1 = DynamicMultiplication.CreateDynamicAddition
                                (
                                    this.WorkingMemory, 
                                    this.additionWorkingMemory,
                                    this.AdditionCompleteHandler
                                );

                                DynamicMultiplication.logger.Info
                                (
                                    new LogMessageGenerator
                                    (
                                        () => 
                                            $"Addition started: [{
                                                string.Join
                                                (
                                                    ',',
                                                    this.WorkingMemory.MultiplierProducts.Lists.Select
                                                    (
                                                        mp =>
                                                            string.Join
                                                            (
                                                                string.Empty,
                                                                mp.Items.Reverse().Select(d => d.Neuron.Tag.Last())
                                                            )
                                                    )
                                                )
                                            }]"
                                    )
                                );
                            }
                        }
                    }
                }
            }
            else
                result = this.DynamicAddition.HandleFire(targetNeuron);

            return result;
        }

        private static DynamicAddition? CreateDynamicAddition
        (
            WorkingMemoryInfo workingMemory, 
            Addition.WorkingMemoryInfo additionWorkingMemoryInfo,
            Action complete
        )
        {
            IListChunk<INeuronChunk> augend;
            if (workingMemory.LastAdditionSum != null)
            {
                augend = workingMemory.LastAdditionSum;
                ArgumentNullException.ThrowIfNull(workingMemory.CurrentMultiplierProduct);
            }
            else
            {
                augend = workingMemory.MultiplierProducts.Lists[0];
                workingMemory.CurrentMultiplierProduct = workingMemory.MultiplierProducts.Lists[1];
            }

            if 
            (
                workingMemory.MultiplierProducts.Lists.IndexOf(workingMemory.CurrentMultiplierProduct) > 
                workingMemory.Product.Value.Count
            )
                workingMemory.Product.Value.Add(augend.Items[0]);

            return new
            (
                new
                (
                    additionWorkingMemoryInfo.PrecedingCarryOverValues,
                    new OperandChunk
                    (
                        additionWorkingMemoryInfo.Augend.DigitValues,
                        new EnumerableChunk<INeuronChunk>([.. augend.Items.Skip(1).Select(ad => new NeuronChunk(ad.Neuron))])
                    ),
                    new OperandChunk
                    (
                        additionWorkingMemoryInfo.Addend.DigitValues,
                        new EnumerableChunk<INeuronChunk>([.. workingMemory.CurrentMultiplierProduct.Items.Select(c => new NeuronChunk(c.Neuron))])
                    ),
                    // Clone without value to ensure IResultChunk.Value is cleared for each DynamicAddition
                    // Otherwise, DynamicAddition will be unable to add to ResultChunk.Value since it is already filled 
                    // in a previously instantiated DynamicAddition
                    ResultChunk.CloneWithoutValue((ResultChunk)additionWorkingMemoryInfo.Sum),
                    ResultChunk.CloneWithoutValue((ResultChunk)additionWorkingMemoryInfo.CarryOver)
                ),
                (a, s) =>
                {
                    // Update lastAdditionSum with sums
                    workingMemory.LastAdditionSum = new ListChunk<INeuronChunk>([.. s.Select(n => new NeuronChunk(n))]);

                    if (workingMemory.CurrentMultiplierProduct != null)
                        DynamicMultiplication.logger.Info
                        (
                            new LogMessageGenerator
                            (
                                () => 
                                    $"Sum [{workingMemory.MultiplierProducts.Lists.IndexOf(workingMemory.CurrentMultiplierProduct)}]: " +
                                    $"{string.Join(string.Empty, s.Reverse().Select(s => s.Tag.Last()))}")
                        );

                    workingMemory.CurrentMultiplierProduct =
                        workingMemory.MultiplierProducts.Lists.GetAdjacentOrDefault(workingMemory.CurrentMultiplierProduct);

                    complete();
                }
            );
        }

        private void AdditionCompleteHandler()
        {
            if (this.WorkingMemory.CurrentMultiplierProduct != null)
            {
                this.Process1 = DynamicMultiplication.CreateDynamicAddition
                    (
                        this.WorkingMemory,
                        this.additionWorkingMemory,
                        this.AdditionCompleteHandler
                    );
            }
            else
            {
                this.Process1 = null;
                this.Complete();
            }
        }

        private void Complete()
        {
            if (this.WorkingMemory.LastAdditionSum != null)
            {
                foreach (var n in this.WorkingMemory.LastAdditionSum.Items)
                    this.WorkingMemory.Product.Value.Add(n);

                this.completionCallback(this, [.. this.WorkingMemory.Product.Value.Items.Select(c => c.Neuron)]);
                this.WorkingMemory.Product.Value.Clear();
            }
        }

        public DynamicAddition? DynamicAddition => this.Process1;
    }
}
