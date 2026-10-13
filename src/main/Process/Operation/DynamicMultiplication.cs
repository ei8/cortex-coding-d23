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

        private readonly IList<NeuronChunk> multiplier;
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
            this.multiplier = [.. this.WorkingMemory.Multiplier.Items];
            this.additionWorkingMemory = additionWorkingMemory;
        }

        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];

            if(this.DynamicAddition == null)
            {
                if 
                (
                    this.WorkingMemory.CurrentMultiplicandDigit != null &&
                    this.WorkingMemory.CurrentMultiplierDigit != null
                )
                    result.AddRange
                    (
                        [
                            this.WorkingMemory.MultiplicandValues.Items.Single(mv => mv.Neuron.Tag.EndsWith(this.WorkingMemory.CurrentMultiplicandDigit.Neuron.Tag.Last())).Neuron,
                            this.WorkingMemory.MultiplierValues.Items.Single(mv => mv.Neuron.Tag.EndsWith(this.WorkingMemory.CurrentMultiplierDigit.Neuron.Tag.Last())).Neuron
                        ]
                    );
            }
            else
                result.AddRange(this.DynamicAddition.GetCurrent());

            return result;
        }

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            if(this.DynamicAddition == null)
            {
                int currentMultiplierDigitIndex = -1;

                if
                (
                    this.WorkingMemory.ProductValues.Items.Any(c => c.Neuron == targetNeuron) &&
                    this.WorkingMemory.LastMultiplicandDigit != this.WorkingMemory.CurrentMultiplicandDigit &&
                    this.WorkingMemory.CurrentMultiplierDigit != null &&
                    (currentMultiplierDigitIndex = this.multiplier.IndexOf(this.WorkingMemory.CurrentMultiplierDigit)) >= 0
                )
                {
                    this.WorkingMemory.LastMultiplicandDigit = this.WorkingMemory.CurrentMultiplicandDigit;
                    
                    if (this.WorkingMemory.MultiplierProducts.Lists.Count < currentMultiplierDigitIndex + 1)
                        this.WorkingMemory.MultiplierProducts.Lists.Add(new ListChunk<NeuronChunk>());

                    this.WorkingMemory.MultiplierProducts.Lists[currentMultiplierDigitIndex].Items.Add(new(targetNeuron));

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

                    if
                    (
                        (
                            this.WorkingMemory.CurrentMultiplicandDigit =
                                this.WorkingMemory.Multiplicand.Items.GetAdjacentOrDefault(this.WorkingMemory.CurrentMultiplicandDigit)
                        ) == null
                    )
                    {
                        if
                        (
                            (
                                this.WorkingMemory.CurrentMultiplierDigit =
                                    this.WorkingMemory.Multiplier.Items.GetAdjacentOrDefault(this.WorkingMemory.CurrentMultiplierDigit)
                            ) != null
                        )
                        {
                            this.WorkingMemory.CurrentMultiplicandDigit = this.WorkingMemory.Multiplicand.Items.First();
                        }
                        else if (this.WorkingMemory.MultiplierProducts.Lists.Count > 0)
                        {
                            if (this.WorkingMemory.MultiplierProducts.Lists.Count == 1)
                            {
                                foreach (var mp in this.WorkingMemory.MultiplierProducts.Lists[0].Items)
                                    this.WorkingMemory.Product.Items.Add(mp);
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
                this.DynamicAddition.HandleFire(targetNeuron, network);
        }

        private static DynamicAddition? CreateDynamicAddition
        (
            WorkingMemoryInfo workingMemory, 
            Addition.WorkingMemoryInfo additionWorkingMemoryInfo,
            Action complete
        )
        {
            IListChunk<NeuronChunk> augend;
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
                workingMemory.Product.Items.Count
            )
                workingMemory.Product.Items.Add(augend.Items[0]);

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
                    workingMemory.LastAdditionSum = new ListChunk<NeuronChunk>([.. s.Select(n => new NeuronChunk(n))]);

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
                    this.WorkingMemory.Product.Items.Add(n);

                this.completionCallback(this, [.. this.WorkingMemory.Product.Items.Select(c => c.Neuron)]);
                this.WorkingMemory.Product.Items.Clear();
            }
        }

        public DynamicAddition? DynamicAddition => this.Process1;
    }
}
