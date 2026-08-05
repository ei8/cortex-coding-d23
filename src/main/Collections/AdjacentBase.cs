using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Collections
{
    public abstract class AdjacentBase : FunctionalCircuitBase<UnaryNeuronParameter>
    {
        protected AdjacentBase(
            FunctionalParameter<UnaryNeuronParameter> parameters,
            ReadOnlyNetwork interneuronNetwork,
            ReadOnlyNetwork linkedInputNeurons,
            VariableInfo? variableInfo
        ) :
        base(
            parameters,
            [
                interneuronNetwork,
                linkedInputNeurons
            ],
            variableInfo
        )
        {
            this.InterneuronNetwork = interneuronNetwork;
        }

        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            FunctionalParameter<UnaryNeuronParameter> parameters,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            params Neuron[] additionalInputs
        ) 
            where T : IAdjacent<T>
        {
            ArgumentOutOfRangeException.ThrowIfNotEqual(parameters.Inputs.Count(), 1);
            ArgumentOutOfRangeException.ThrowIfNotEqual(parameters.Outputs.Count(), 1);

            bool bResult = false;
            result = default;

            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                var interneuronNetwork = T.CreateInterneuronNetwork(
                    parameters,
                    variableInfo
                );
                result = T.Create(
                    parameters,
                    interneuronNetwork,
                    Next.LinkInputNeurons(
                        interneuronNetwork,
                        parameters,
                        additionalInputs
                    ),
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }
        
        protected static ReadOnlyNetwork LinkInputNeurons(
            UnaryNeuronParameter input,
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

        public ReadOnlyNetwork InterneuronNetwork { get; }
    }
}
