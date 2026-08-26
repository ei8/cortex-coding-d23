using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        // TODO: transfer all fields and parameters into working memory as chunks
        private readonly IEnumerable<Neuron> multiplicand;
        private readonly IEnumerable<Neuron> multiplier;
        private int currentMultiplicandDigit; 
        private int currentMultiplierDigit;
        private bool isMultiplying;
        // TODO: isMultiplying can be changed into a function that returns true if lastMultiplicandDigit and lastMultiplierDigit are actually lasts in multiplicand and multiplier
        private int currentMultiplierProduct; // TODO: can use IList<IListChunk> ie. NestedListChunk.Content
        private int currentAdditionDigit;
        private IListChunk? lastAdditionSum;
        private int? lastMultiplicandDigit;

        [SetsRequiredMembers]
        public DynamicMultiplication
        (
            IEnumerable<Neuron> multiplicand,
            IEnumerable<Neuron> multiplier,
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
            this.Process1 = new
            (
                additionWorkingMemory,
                () => this.currentAdditionDigit,
                () => this.currentAdditionDigit++,
                (i, wm) =>
                {
                    List<Neuron> result = [];

                    if (this.WorkingMemory.MultiplierProducts.Content.Count > 0)
                    {
                        if (this.WorkingMemory.MultiplierProducts.Content.Count == 1)
                        {
                            foreach (var mp in this.WorkingMemory.MultiplierProducts.Content[0].Content)
                                this.WorkingMemory.Product.Content.Add(mp);
                            this.Complete();
                        }
                        else
                        {
                            if (this.currentMultiplierProduct == 0)
                                this.currentMultiplierProduct++;

                            IListChunk addend1;

                            if (lastAdditionSum != null)
                                addend1 = this.lastAdditionSum;
                            else
                                addend1 = this.WorkingMemory.MultiplierProducts.Content[this.currentMultiplierProduct - 1];

                            if (this.currentMultiplierProduct > this.WorkingMemory.Product.Content.Count)
                                this.WorkingMemory.Product.Content.Add(addend1.Content[0]);
                            
                            var addend1Array = addend1.Content.Skip(1).ToArray();

                            IList<Neuron[]> addends =
                            [
                                addend1Array,
                                [.. this.WorkingMemory.MultiplierProducts.Content[this.currentMultiplierProduct].Content]
                            ];

                            if (addends.Any(a => i < a.Length))
                                foreach (var addend in addends)
                                {
                                    EnumerableChunk values;
                                    if (addends.IndexOf(addend) == 0)
                                        values = wm.Addend1Values;
                                    else
                                        values = wm.Addend2Values;

                                    if (i < addend.Length)
                                        result.Add(values.Content.Single(ad => ad.Tag.EndsWith(addend[i].Tag.Last())));
                                    else
                                        result.Add(values.Content.Single(ad => ad.Tag.EndsWith('0')));
                                }
                        }
                    }

                    return result;
                },
                (a, s) =>
                {
                    // Update lastAdditionSum with sums
                    this.lastAdditionSum = new ListChunk([.. s]);
                    // Reset currentAdditionDigit 
                    this.currentAdditionDigit = 0;

                    DynamicMultiplication.logger.Info
                    (
                        new LogMessageGenerator(() => $"Sum [{this.currentMultiplierProduct}]: {string.Join(string.Empty, s.Reverse().Select(s => s.Tag.Last()))}")
                    );

                    if (DynamicMultiplication.IncrementReset(ref this.currentMultiplierProduct, this.WorkingMemory.MultiplierProducts.Content.Count))
                    {
                        foreach (var n in this.lastAdditionSum.Content)
                            this.WorkingMemory.Product.Content.Add(n);
                        this.Complete();
                    }
                }
            );
            this.multiplicand = multiplicand;
            this.multiplier = multiplier;

            this.currentMultiplicandDigit = 0;
            this.currentMultiplierDigit = 0;

            this.isMultiplying = true;

            this.currentMultiplierProduct = 0;
            this.currentAdditionDigit = 0;
            this.lastAdditionSum = null;

            this.lastMultiplicandDigit = null;
        }

        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];

            if (this.isMultiplying)
            {
                result.AddRange
                (
                    [
                        this.WorkingMemory.MultiplicandValues.Content.Single(mv => mv.Tag.EndsWith(this.multiplicand.ElementAt(this.currentMultiplicandDigit).Tag.Last())),
                        this.WorkingMemory.MultiplierValues.Content.Single(mv => mv.Tag.EndsWith(this.multiplier.ElementAt(this.currentMultiplierDigit).Tag.Last()))
                    ]
                );
            }
            else
                result.AddRange(this.DynamicAddition.GetCurrent());

            return result;
        }

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            if (this.isMultiplying)
            {
                if
                (
                    this.WorkingMemory.ProductValues.Content.Contains(targetNeuron) &&
                    this.lastMultiplicandDigit != this.currentMultiplicandDigit
                )
                {
                    this.lastMultiplicandDigit = this.currentMultiplicandDigit;
                    
                    if (this.WorkingMemory.MultiplierProducts.Content.Count < this.currentMultiplierDigit + 1)
                        this.WorkingMemory.MultiplierProducts.Content.Add(new ListChunk());

                    this.WorkingMemory.MultiplierProducts.Content[this.currentMultiplierDigit].Content.Add(targetNeuron);

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
                                            .Content[this.currentMultiplierDigit]
                                            .Content.Reverse().Select(d => d.Tag.Last())
                                    )
                                }"
                        )
                    );

                    if 
                    (
                        DynamicMultiplication.IncrementReset(ref this.currentMultiplicandDigit, this.multiplicand.Count()) &&
                        DynamicMultiplication.IncrementReset(ref this.currentMultiplierDigit, this.multiplier.Count())
                    )
                    {
                        this.isMultiplying = false;

                        DynamicMultiplication.logger.Info
                        (
                            new LogMessageGenerator
                            (
                                () => $"Addition started: " +
                                    $"[{string.Join
                                        (
                                            ',',
                                            this.WorkingMemory.MultiplierProducts
                                                .Content.Select
                                                (
                                                    mp => 
                                                        string.Join
                                                        (
                                                            string.Empty, 
                                                            mp.Content.Reverse().Select(d => d.Tag.Last())
                                                        )
                                                )
                                    )}]"
                            )
                        );
                    }
                }
            }
            else
                this.DynamicAddition.HandleFire(targetNeuron, network);
        }

        private static bool IncrementReset(ref int value, int max)
        {
            var result = false;

            value++;

            if (value >= max)
            {
                value = 0;
                result = true;
            }

            return result;
        }

        private void Complete()
        {
            this.completionCallback(this, [.. this.WorkingMemory.Product.Content]);
            this.WorkingMemory.Product.Content.Clear();
        }

        public DynamicAddition DynamicAddition => this.Process1;
    }
}
