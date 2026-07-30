using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Collections
{
    public abstract class AdjacentBase : FunctionalCircuitBase<UnaryNeuronInfo>
    {
        public AdjacentBase() : base()
        {
        }

        protected abstract string GetInterneuronTag(VariableInfo variableInfo);

        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            FunctionalParameter<UnaryNeuronInfo> parameters,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            params Neuron[] additionalInputs
        ) where T : AdjacentBase, new()
        {
            bool bResult = false;
            result = null;

            var output = parameters.Outputs.Single();
            var input = parameters.Inputs.Single();

            if (
                VariableInfo.TryParse(parameterExpression, out var variableInfo) &&
                output != null &&
                input != null
            )
            {
                result = new();
                var interneuronNetwork = NetworkHelper.CreateInterneuronNetworks(
                    [output.Neuron],
                    [result.GetInterneuronTag(variableInfo)]
                );
                result.Initialize(
                    parameters,
                    [
                        ..interneuronNetwork,
                        AdjacentBase.LinkInputNeuron(
                            input,
                            interneuronNetwork.Single(),
                            additionalInputs
                        )
                    ]
                );
                bResult = true;
            }

            return bResult;
        }
        
        private static ReadOnlyNetwork LinkInputNeuron(
            UnaryNeuronInfo input,
            ReadOnlyNetwork interneuronNetwork,
            params Neuron[] additionalInputs
        ) =>
            NetworkHelper.LinkInputNeuronsToInterneuron(
                interneuronNetwork.GetInterneuron(),
                [
                    input.Neuron,
                    .. additionalInputs
                ]
            );
    }
}
