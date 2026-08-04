using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class LogicGateBase : FunctionalCircuitBase<BinaryNeuronParameter>
    {
        public LogicGateBase() : base()
        {
        }

        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            FunctionalParameter<BinaryNeuronParameter> parameters,
            InterneuronTagInfo? interneuronTagInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            params Neuron[] additionalInputs
        ) 
            where T : LogicGateBase, new()
        {
            bool bResult = false;
            result = null;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                result = new();
                var output = parameters.Outputs.Single();
                var interneuronNetworks = Enumerable.Empty<ReadOnlyNetwork>();
                if (output != null)
                    interneuronNetworks = NetworkHelper.CreateInterneuronNetworksByOutputNeurons(
                        result.GetInterneuronOutputs(output),
                        result.GetInterneuronTags(variableInfo, interneuronTagInfo)
                    );
                
                result.Initialize(
                    parameters,
                    [
                        ..interneuronNetworks,
                        ..result.LinkInputNeurons(
                            parameters.Inputs.WhereNotNull(),
                            interneuronNetworks,
                            additionalInputs
                        )
                    ]
                );
                result.VariableInfo = variableInfo;
                bResult = true;
            }

            return bResult;
        }

        protected abstract IEnumerable<Neuron> GetInterneuronOutputs(BinaryNeuronParameter output);

        protected abstract IEnumerable<string> GetInterneuronTags(
            VariableInfo variableInfo,
            InterneuronTagInfo? interneuronTagInfo = null
        );

        protected abstract IEnumerable<ReadOnlyNetwork> LinkInputNeurons(
            IEnumerable<BinaryNeuronParameter> inputs,
            IEnumerable<ReadOnlyNetwork> interneuronNetworks,
            params Neuron[] additionalInputs
        );
    }
}