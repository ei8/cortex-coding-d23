using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class LogicGateBase : FunctionalCircuitBase<BinaryNeuronParameter>
    {
        protected LogicGateBase(
            FunctionalParameter<BinaryNeuronParameter> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) : base(
            parameters,
            networks,
            variableInfo
        )
        {
        }

        public static bool TryCreate<T>(
            [NotNullWhen(true)] out T? result,
            FunctionalParameter<BinaryNeuronParameter> parameters,
            InterneuronTagInfo? interneuronTagInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            params Neuron[] additionalInputs
        ) 
            where T : ILogicGate<T>
        {
            bool bResult = false;
            result = default;
            if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
            {
                var output = parameters.Outputs.Single();
                var interneuronNetworks = Enumerable.Empty<ReadOnlyNetwork>();
                if (output != null)
                    interneuronNetworks = NetworkHelper.CreateInterneuronNetworksByOutputNeurons(
                        T.GetInterneuronOutputs(output),
                        T.GetInterneuronTags(variableInfo, interneuronTagInfo)
                    );
                
                result = T.Create(
                    parameters,
                    [
                        ..interneuronNetworks,
                        ..T.LinkInputNeurons(
                            parameters.Inputs.WhereNotNull(),
                            interneuronNetworks,
                            additionalInputs
                        )
                    ],
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }
    }
}