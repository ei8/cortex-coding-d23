using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Selectors;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
{
    public class UnitProcessor : IUnitProcessor
    {
        public bool TryParse(Ensemble ensemble, Id23neurULizerReadOptions options, IUnitParameterSet parameters, out IUnit result)
        {
            result = null;

            if (parameters.Type != null)
            {
                var tempResult = new Unit();

                if (parameters.Granny != null)
                {
                    tempResult.Neuron = parameters.Granny;
                    tempResult.Type = parameters.Type;

                    this.TryParseCore(
                        ensemble,
                        tempResult,
                        new[] { tempResult.Neuron.Id },
                        new[] { new LevelParser(new PostsynapticByPostsynapticSibling(tempResult.Type.Id)) },
                        (n) => tempResult.Value = n,
                        ref result
                        );

                    if (
                        result != null &&
                        parameters.Value != null &&
                        result.Value != null &&
                        parameters.Value.Id != result.Value.Id
                        )
                        result = null;
                }
                else if (parameters.Value != null)
                {
                    tempResult.Value = parameters.Value;
                    tempResult.Type = parameters.Type;

                    this.TryParseCore(
                        ensemble,
                        tempResult,
                        new[] { tempResult.Value.Id },
                        new[] { new LevelParser(new PresynapticByPostsynapticSibling(tempResult.Type.Id)) },
                        (n) => tempResult.Neuron = n,
                        ref result
                        );
                }
            }

            return result != null;
        }
    }
}
