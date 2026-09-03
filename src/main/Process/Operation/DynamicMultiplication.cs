using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Process.Operation
{
    public partial class DynamicMultiplication :
        FiniteCompositeProcessBase
        <
            DynamicMultiplication,
            DynamicMultiplication.WorkingMemoryInfo,
            DynamicAddition,
            Action<DynamicMultiplication, IEnumerable<Neuron>>
        >
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IList<NeuronChunk> multiplier;
        private readonly Addition.WorkingMemoryValuesInfo additionWorkingMemory;

        public DynamicMultiplication
        (
            DynamicMultiplication.WorkingMemoryInfo workingMemory,
            Addition.WorkingMemoryValuesInfo additionWorkingMemory,
            Action<DynamicMultiplication, IEnumerable<Neuron>> completionCallback
        ) :
            base
            (
                workingMemory,
                completionCallback
            )
        {
            this.multiplier = [.. this.WorkingMemory.Multiplier.Content];
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
                            this.WorkingMemory.MultiplicandValues.Content.Single(mv => mv.Value.Tag.EndsWith(this.WorkingMemory.CurrentMultiplicandDigit.Content.Tag.Last())).Value,
                            this.WorkingMemory.MultiplierValues.Content.Single(mv => mv.Value.Tag.EndsWith(this.WorkingMemory.CurrentMultiplierDigit.Content.Tag.Last())).Value
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
                    this.WorkingMemory.ProductValues.Content.Any(c => c.Value == targetNeuron) &&
                    this.WorkingMemory.LastMultiplicandDigit != this.WorkingMemory.CurrentMultiplicandDigit &&
                    this.WorkingMemory.CurrentMultiplierDigit != null &&
                    (currentMultiplierDigitIndex = this.multiplier.IndexOf(this.WorkingMemory.CurrentMultiplierDigit)) >= 0
                )
                {
                    this.WorkingMemory.LastMultiplicandDigit = this.WorkingMemory.CurrentMultiplicandDigit;
                    
                    if (this.WorkingMemory.MultiplierProducts.Content.Count < currentMultiplierDigitIndex + 1)
                        this.WorkingMemory.MultiplierProducts.Content.Add(new ListChunk<NeuronChunk>());

                    this.WorkingMemory.MultiplierProducts.Content[currentMultiplierDigitIndex].Content.Add(new(targetNeuron));

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
                                            .Content[currentMultiplierDigitIndex]
                                            .Content.Reverse().Select(d => d.Value.Tag.Last())
                                    )
                                }"
                        )
                    );

                    if
                    (
                        (
                            this.WorkingMemory.CurrentMultiplicandDigit =
                                this.WorkingMemory.Multiplicand.Content.IncrementReset(this.WorkingMemory.CurrentMultiplicandDigit)
                        ) == null
                    )
                    {
                        if
                        (
                            (
                                this.WorkingMemory.CurrentMultiplierDigit =
                                    this.WorkingMemory.Multiplier.Content.IncrementReset(this.WorkingMemory.CurrentMultiplierDigit)
                            ) != null
                        )
                        {
                            this.WorkingMemory.CurrentMultiplicandDigit = this.WorkingMemory.Multiplicand.Content.First();
                        }
                        else if (this.WorkingMemory.MultiplierProducts.Content.Count > 0)
                        {
                            if (this.WorkingMemory.MultiplierProducts.Content.Count == 1)
                            {
                                foreach (var mp in this.WorkingMemory.MultiplierProducts.Content[0].Content)
                                    this.WorkingMemory.Product.Content.Add(mp);
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
                                                    this.WorkingMemory.MultiplierProducts.Content.Select
                                                    (
                                                        mp =>
                                                            string.Join
                                                            (
                                                                string.Empty,
                                                                mp.Content.Reverse().Select(d => d.Value.Tag.Last())
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
            Addition.WorkingMemoryValuesInfo additionWorkingMemoryValuesInfo,
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
                augend = workingMemory.MultiplierProducts.Content[0];
                workingMemory.CurrentMultiplierProduct = workingMemory.MultiplierProducts.Content[1];
            }

            if (workingMemory.GetCurrentMultiplierProductIndex() > workingMemory.Product.Content.Count)
                workingMemory.Product.Content.Add(augend.Content[0]);

            return new
            (
                new
                (
                    additionWorkingMemoryValuesInfo.PrecedingCarryOverValues,
                    additionWorkingMemoryValuesInfo.AugendValues,
                    additionWorkingMemoryValuesInfo.AddendValues,
                    new([.. augend.Content.Skip(1).Select(ad => new NeuronChunk(ad.Value))]),
                    new([.. workingMemory.CurrentMultiplierProduct.Content.Select(c => new NeuronChunk(c.Value))]),
                    additionWorkingMemoryValuesInfo.SumValues,
                    additionWorkingMemoryValuesInfo.CarryOverValues
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
                                    $"Sum [{workingMemory.GetCurrentMultiplierProductIndex()}]: " +
                                    $"{string.Join(string.Empty, s.Reverse().Select(s => s.Tag.Last()))}")
                        );

                    workingMemory.CurrentMultiplierProduct =
                        workingMemory.MultiplierProducts.Content.IncrementReset(workingMemory.CurrentMultiplierProduct);

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
                foreach (var n in this.WorkingMemory.LastAdditionSum.Content)
                    this.WorkingMemory.Product.Content.Add(n);

                this.completionCallback(this, [.. this.WorkingMemory.Product.Content.Select(c => c.Value)]);
                this.WorkingMemory.Product.Content.Clear();
            }
        }

        public DynamicAddition? DynamicAddition => this.Process1;
    }
}
