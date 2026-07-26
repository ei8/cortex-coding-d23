using System;

namespace ei8.Cortex.Coding.d23
{
    public class StimulusParser
    {
        public StimulusParser(StimulusType type, Predicate<object> evaluator, Func<object, Neuron> neuronConverter)
        {
            this.Type = type;
            this.Evaluator = evaluator;
            this.NeuronConverter = neuronConverter;
        }

        public StimulusType Type { get; }

        public Predicate<object> Evaluator { get; }

        public Func<object, Neuron> NeuronConverter { get; }
    } 
}
