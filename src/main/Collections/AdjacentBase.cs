using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Collections
{
    public abstract class AdjacentBase : FunctionalCircuitBase<UnaryNeuronParameter>
    {
        public AdjacentBase() : base()
        {
        }

        protected abstract IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworks(
            FunctionalParameter<UnaryNeuronParameter> parameters,
            VariableInfo variableInfo,
            params Neuron[] additionalInputs
        );

        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            FunctionalParameter<UnaryNeuronParameter> parameters,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            params Neuron[] additionalInputs
        ) where T : AdjacentBase, new()
        {
            ArgumentOutOfRangeException.ThrowIfNotEqual(parameters.Inputs.Count(), 2);
            ArgumentOutOfRangeException.ThrowIfNotEqual(parameters.Outputs.Count(), 2);

            bool bResult = false;
            result = null;

            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                result = new();
                result.Initialize(
                    parameters,
                    result.CreateInterneuronNetworks(
                        parameters,
                        variableInfo,
                        additionalInputs
                    )
                );
                bResult = true;
            }

            return bResult;
        }
        
        protected static ReadOnlyNetwork LinkInputNeuron(
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
    }
}
