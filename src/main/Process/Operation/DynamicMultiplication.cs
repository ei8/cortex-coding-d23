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

        private readonly IList<NeuronChunk> multiplier;

        [SetsRequiredMembers]
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
            this.Process1 = new
            (
                additionWorkingMemory,
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
                            if (this.WorkingMemory.CurrentMultiplierProduct == null)
                                this.WorkingMemory.CurrentMultiplierProduct = this.WorkingMemory.MultiplierProducts.Content[1];

                            IListChunk<NeuronChunk> addend1;

                            var currentMultiplierProductIndex = this.GetCurrentMultiplierProductIndex(this.WorkingMemory.CurrentMultiplierProduct);

                            if (this.WorkingMemory.LastAdditionSum != null)
                                addend1 = this.WorkingMemory.LastAdditionSum;
                            else
                                addend1 = this.WorkingMemory.MultiplierProducts.Content[currentMultiplierProductIndex - 1];

                            if (currentMultiplierProductIndex > this.WorkingMemory.Product.Content.Count)
                                this.WorkingMemory.Product.Content.Add(addend1.Content[0]);

                            var addend1Array = addend1.Content.Skip(1).ToArray();

                            IList<Neuron[]> addends =
                            [
                                [.. addend1Array.Select(ad => ad.Value)],
                                [.. this.WorkingMemory.MultiplierProducts.Content[currentMultiplierProductIndex].Content.Select(c => c.Value)]
                            ];

                            if (addends.Any(a => i < a.Length))
                                foreach (var addend in addends)
                                {
                                    EnumerableChunk<NeuronChunk> values;
                                    if (addends.IndexOf(addend) == 0)
                                        values = wm.Addend1Values;
                                    else
                                        values = wm.Addend2Values;

                                    if (i < addend.Length)
                                        result.Add(values.Content.Single(ad => ad.Value.Tag.EndsWith(addend[i].Tag.Last())).Value);
                                    else
                                        result.Add(values.Content.Single(ad => ad.Value.Tag.EndsWith('0')).Value);
                                }
                        }
                    }

                    return result;
                },
                (a, s) =>
                {
                    // Update lastAdditionSum with sums
                    this.WorkingMemory.LastAdditionSum = new ListChunk<NeuronChunk>([.. s.Select(n => new NeuronChunk(n))]);

                    if (this.WorkingMemory.CurrentMultiplierProduct != null)
                        DynamicMultiplication.logger.Info
                        (
                            new LogMessageGenerator(() => $"Sum [{this.GetCurrentMultiplierProductIndex(this.WorkingMemory.CurrentMultiplierProduct)}]: {string.Join(string.Empty, s.Reverse().Select(s => s.Tag.Last()))}")
                        );

                    if 
                    (
                        this.WorkingMemory.LastAdditionSum != null &&
                        (
                            this.WorkingMemory.CurrentMultiplierProduct = 
                                DynamicMultiplication.IncrementReset
                                (
                                    this.WorkingMemory.CurrentMultiplierProduct, 
                                    this.WorkingMemory.MultiplierProducts.Content
                                )
                        ) == null
                    )
                    {
                        foreach (var n in this.WorkingMemory.LastAdditionSum.Content)
                            this.WorkingMemory.Product.Content.Add(n);
                        this.Complete();
                    }
                }
            );

            this.multiplier = [.. this.WorkingMemory.Multiplier.Content];
        }

        private int GetCurrentMultiplierProductIndex(IListChunk<NeuronChunk> currentListChunk)
        {
            return this.WorkingMemory.MultiplierProducts.Content.IndexOf(currentListChunk);
        }

        public override IEnumerable<Neuron> GetCurrent()
        {
            List<Neuron> result = [];

            if
            (
                DynamicMultiplication.IsMultiplying
                (
                    this.WorkingMemory.CurrentMultiplicandDigit, 
                    this.WorkingMemory.CurrentMultiplierDigit
                )
            )
            {
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

        private static bool IsMultiplying
        (
            [NotNullWhen(true)]
            NeuronChunk? multiplicandDigit,
            [NotNullWhen(true)]
            NeuronChunk? multiplierDigit
        ) =>
            multiplicandDigit != null &&
            multiplierDigit != null;

        public override void HandleFire(Neuron targetNeuron, ReadOnlyNetwork network)
        {
            if 
            (
                DynamicMultiplication.IsMultiplying
                (
                    this.WorkingMemory.CurrentMultiplicandDigit,
                    this.WorkingMemory.CurrentMultiplierDigit
                )
            )
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
                                DynamicMultiplication.IncrementReset
                                (
                                    this.WorkingMemory.CurrentMultiplicandDigit,
                                    this.WorkingMemory.Multiplicand.Content
                                )
                        ) == null
                    )
                    {
                        if
                        (
                            (
                                this.WorkingMemory.CurrentMultiplierDigit =
                                    DynamicMultiplication.IncrementReset
                                    (
                                        this.WorkingMemory.CurrentMultiplierDigit,
                                        this.WorkingMemory.Multiplier.Content
                                    )
                            ) != null
                        )
                        {
                            this.WorkingMemory.CurrentMultiplicandDigit = this.WorkingMemory.Multiplicand.Content.First();
                        }
                        else
                        {
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
                                                                mp.Content.Reverse().Select(d => d.Value.Tag.Last())
                                                            )
                                                    )
                                        )}]"
                                )
                            );
                        }
                    }
                }
            }
            else
                this.DynamicAddition.HandleFire(targetNeuron, network);
        }

        private static T? IncrementReset<T>(T? item, IEnumerable<T> list)
            where T : class
        {
            T? nextItem = default;
            
            if (item != null)
                nextItem = list
                    .SkipWhile(li => li != item)
                    .Skip(1)
                    .FirstOrDefault();

            return nextItem;
        }

        private void Complete()
        {
            this.completionCallback(this, [.. this.WorkingMemory.Product.Content.Select(c => c.Value)]);
            this.WorkingMemory.Product.Content.Clear();
        }

        public DynamicAddition DynamicAddition => this.Process1;
    }
}
