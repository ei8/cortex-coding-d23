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

        private readonly Addition.WorkingMemoryInfo? additionWorkingMemory;

        public DynamicMultiplication
        (
            DynamicMultiplication.WorkingMemoryInfo workingMemory,
            Action<DynamicMultiplication, IEnumerable<Neuron>> completionCallback
        ) :
            this
            (
                workingMemory,
                null,
                completionCallback
            )
        {
        }

        public DynamicMultiplication
        (
            DynamicMultiplication.WorkingMemoryInfo workingMemory,
            Addition.WorkingMemoryInfo? additionWorkingMemory,
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

            // TODO: Use Next to determine current operation
            if(this.DynamicAddition != null)
                result.AddRange(this.DynamicAddition.GetCurrent());
            else
                result.AddRange(this.WorkingMemory.GetCurrent().Select(c => c.Neuron));

            return result;
        }

        public override bool HandleFire(Neuron targetNeuron)
        {
            ArgumentNullException.ThrowIfNull(this.WorkingMemory.Multiplier.Value);

            var result = false;

            if (this.DynamicAddition != null)
                result = this.DynamicAddition.HandleFire(targetNeuron);
            else
            {
                if 
                (
                    // if target is a product digit
                    this.WorkingMemory.Product.DigitValues.Items.Any(c => c.Neuron == targetNeuron) &&
                    // and current multiplier index exists
                    this.WorkingMemory.Multiplier.ValueArray.TryGetIndexOf
                    (
                        this.WorkingMemory.Multiplier.CurrentDigit,
                        out var currentMultiplierDigitIndex
                    )
                )
                {
                    result = true; 

                    // update multiplicand current digit
                    this.WorkingMemory.Multiplicand.UpdateTracking();

                    // if multiplier product does not yet exist for current multiplier digit
                    if (this.WorkingMemory.MultiplierProducts.Lists.Count < currentMultiplierDigitIndex + 1)
                        // create and new multiplier product
                        this.WorkingMemory.MultiplierProducts.Lists.Add(new ListChunk<INeuronChunk>());

                    // add product to current multiplier product
                    this.WorkingMemory.MultiplierProducts.Lists[currentMultiplierDigitIndex.Value].Items.Add(new NeuronChunk(targetNeuron));

                    DynamicMultiplication.logger.Info
                    (
                        new LogMessageGenerator
                        (
                            () =>
                                $"Added to MultiplierProduct(s) [Index:{currentMultiplierDigitIndex}]: {targetNeuron.Tag}, " +
                                $"{
                                    string.Join
                                    (
                                        string.Empty,
                                        this.WorkingMemory.MultiplierProducts
                                            .Lists[currentMultiplierDigitIndex.Value]
                                            .Items.Reverse().Select(d => d.Neuron.Tag.Last())
                                    )
                                }"
                        )
                    );

                    // if multiplicand is past left-most digit
                    if (this.WorkingMemory.Multiplicand.CurrentDigit == null)
                    {
                        // update multiplier current digit
                        this.WorkingMemory.Multiplier.UpdateTracking();
                        // if multiplier is not left-most digit
                        if (this.WorkingMemory.Multiplier.CurrentDigit != null)
                        {
                            // reset multiplicand digit to right-most digit
                            this.WorkingMemory.Multiplicand.ResetTracking();
                        }
                        // ... otherwise if multiplier products have accumulated
                        else if (this.WorkingMemory.MultiplierProducts.Lists.Count > 0)
                        {
                            // if it's just one multiplier product or
                            // additionWorkingMemory was not specified
                            if 
                            (
                                this.WorkingMemory.MultiplierProducts.Lists.Count == 1 ||
                                this.additionWorkingMemory == null
                            )
                            {
                                foreach (var mp in this.WorkingMemory.MultiplierProducts.Lists[0].Items)
                                    this.WorkingMemory.Product.Value.Add(mp);
                                this.Complete();
                            }
                            // otherwise, if more
                            else
                            {
                                this.DynamicAddition = DynamicMultiplication.CreateDynamicAddition
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
                                            $"Addition started: [{string.Join
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
                                                )}]"
                                    )
                                );
                            }
                        }
                    }
                }
            }

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
            // if multiplier products were previously added
            if (workingMemory.LastAdditionSum != null)
            {
                // use last sum as augend
                augend = workingMemory.LastAdditionSum;
                // ensure current multiplier product is set
                ArgumentNullException.ThrowIfNull(workingMemory.CurrentMultiplierProduct);
            }
            // otherwise
            else
            {
                // use first multiplier product as augend
                augend = workingMemory.MultiplierProducts.Lists[0];
                // use second multiplier product as current multiplier product
                workingMemory.CurrentMultiplierProduct = workingMemory.MultiplierProducts.Lists[1];
            }

            // if current multiplier product is already past count of product digits
            if 
            (
                workingMemory.MultiplierProducts.Lists.IndexOf(workingMemory.CurrentMultiplierProduct) > 
                workingMemory.Product.Value.Count
            )
                // add least digit of augend to product
                workingMemory.Product.Value.Add(augend.Items[0]);

            return new
            (
                new
                (
                    additionWorkingMemoryInfo.PrecedingCarryOverValues,
                    // use remaining (skipping least digit) digits of augend as augend
                    new OperandChunk
                    (
                        additionWorkingMemoryInfo.Augend.DigitValues,
                        new EnumerableChunk<INeuronChunk>([.. augend.Items.Skip(1).Select(ad => new NeuronChunk(ad.Neuron))])
                    ),
                    // use current multiplier product as addend
                    new OperandChunk
                    (
                        additionWorkingMemoryInfo.Addend.DigitValues,
                        new EnumerableChunk<INeuronChunk>([.. workingMemory.CurrentMultiplierProduct.Items.Select(c => new NeuronChunk(c.Neuron))])
                    ),
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
                        workingMemory.MultiplierProducts.Lists.ToArray().GetAdjacentOrDefault(workingMemory.CurrentMultiplierProduct);

                    complete();
                }
            );
        }

        private void AdditionCompleteHandler()
        {
            if 
            (
                this.WorkingMemory.CurrentMultiplierProduct != null && 
                this.additionWorkingMemory != null
            )
            {
                this.DynamicAddition = DynamicMultiplication.CreateDynamicAddition
                    (
                        this.WorkingMemory,
                        this.additionWorkingMemory,
                        this.AdditionCompleteHandler
                    );
            }
            else
            {
                this.DynamicAddition = null;
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

        public DynamicAddition? DynamicAddition 
        {
            get => this.Process1; 
            private set { this.Process1 = value; }
        }
    }
}
