using System;
using System.Collections.Generic;
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
            ReadOnlyNetwork? precedingInterneuronNetwork = null,
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
                    T.LinkInputNeurons(
                        interneuronNetwork,
                        parameters,
                        precedingInterneuronNetwork,
                        additionalInputs
                    ),
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }

        protected static ReadOnlyNetwork LinkInputNeurons(
            UnaryNeuronParameter current,
            ReadOnlyNetwork interneuronNetwork,
            ReadOnlyNetwork? precedingInterneuronNetwork = null,
            params Neuron[] additionalInputs
        )
        {
            var inputNeurons = new List<NeuronInfo>(
                [
                    new(current.Neuron),
                    .. additionalInputs.Select(n => new NeuronInfo(n))
                ]);
            if (precedingInterneuronNetwork != null)
                inputNeurons.Add(new NeuronInfo(precedingInterneuronNetwork.GetInterneuron(), 1f, NeurotransmitterEffect.Inhibit));

            return NetworkHelper.LinkInputNeuronsToInterneuron(
                interneuronNetwork.GetInterneuron(),
                [.. inputNeurons]
            );
        }

        public ReadOnlyNetwork InterneuronNetwork { get; }
    }
}
