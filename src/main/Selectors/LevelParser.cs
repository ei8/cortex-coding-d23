using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Selectors
{
    public class LevelParser
    {
        public LevelParser(params ISelector[] selectors)
        {
            this.Selectors = selectors;
        }

        public IEnumerable<ISelector> Selectors { get; private set; }

        public IEnumerable<Neuron> Evaluate(Ensemble ensemble, IEnumerable<Neuron> selection)
        {
            foreach (var selector in this.Selectors)
                selection = selector.Evaluate(ensemble, selection);

            return selection;
        }
    }
}
