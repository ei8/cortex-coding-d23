using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public abstract partial class TwoInputOneOutputTruthTableBase
    (
        TwoInputOneOutputTruthTableBase.ParameterInfo parameters,
        TwoInputTruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TwoInputTruthTableBase
        <
            TwoInputOneOutputTruthTableBase.ParameterInfo,
            TwoInputTruthTableBase.Input,
            TwoInputTruthTableBase.InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        )
    {
        public static bool TryCreate<T>
        (
            [NotNullWhen(true)] out T? result,
            TwoInputOneOutputTruthTableBase.ParameterInfo parameters,
            InterneuronTagInfo? interneuronTagInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            NetworkHelper.InputNeuronStrengthMode additionalInputNeuronType = NetworkHelper.InputNeuronStrengthMode.And,
            params Neuron[] additionalInputs
        )
            where T :
                ITruthTableStatic
                <
                    T,
                    TwoInputOneOutputTruthTableBase.ParameterInfo,
                    TwoInputTruthTableBase.Input,
                    TwoInputOneOutputTruthTableBase.Output,
                    TwoInputTruthTableBase.InterneuronSet
                >
        {
            bool bResult = false;
            result = default;
            if 
            (
                VariableInfo.TryParse(parameterExpression, out var variableInfo) &&
                parameters.Outputs.Output1 != null &&
                parameters.Inputs.Input1 != null &&
                parameters.Inputs.Input2 != null
            )
            {
                var interneuronNetworks = NetworkHelper.CreateInterneuronNetworksByOutputNeurons
                (
                    T.GetOutputsPerInterneuron(parameters.Outputs),
                    T.GetInterneuronTags(variableInfo, interneuronTagInfo)
                );

                var interneurons = new TwoInputTruthTableBase.InterneuronSet
                (
                    interneuronNetworks.ElementAt(0),
                    interneuronNetworks.ElementAt(1),
                    interneuronNetworks.ElementAt(2),
                    interneuronNetworks.ElementAt(3),
                    T.LinkInputNeurons
                    (
                        parameters.Inputs,
                        interneuronNetworks,
                        additionalInputNeuronType,
                        additionalInputs
                    ).Combine()
                );

                result = T.Create
                (
                    parameters,
                    interneurons,
                    variableInfo
                );
                bResult = true;
            }

            return bResult;
        }
    }
}
