using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Math.TruthTables
{
    public abstract partial class TwoInputThreeOutputTruthTableBase
    (
        TwoInputThreeOutputTruthTableBase.ParameterInfo parameters,
        TwoInputTruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TwoInputThreeOutputTruthTableBase
        <
            TwoInputThreeOutputTruthTableBase.ParameterInfo,
            TwoInputTruthTableBase.Input,
            TwoInputThreeOutputTruthTableBase.Output
        >
        (
            parameters,
            interneurons,
            variableInfo
        )
    {
    }

    public abstract partial class TwoInputThreeOutputTruthTableBase
    <
        TParam,
        TInput,
        TOutput
    >
    (
        TParam parameters,
        TwoInputTruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TwoInputTruthTableBase
        <
            TParam,
            TInput,
            TwoInputTruthTableBase.InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        )
        where TParam : IFunctionalCircuitParameter<TInput, TOutput>
        where TInput :
            IInputCircuitParameterSubset
            <
                BinaryNeuronParameter,
                BinaryNeuronParameter
            >
        where TOutput : 
            IOutputCircuitParameterSubset
            <
                BinaryNeuronParameter,
                BinaryNeuronParameter,
                BinaryNeuronParameter
            >
    {
        public static bool TryCreate<T>
        (
            [NotNullWhen(true)] out T? result,
            TParam parameters,
            InterneuronTagInfo? interneuronTagInfo = null,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
            NetworkHelper.InputNeuronStrengthMode additionalInputNeuronType = NetworkHelper.InputNeuronStrengthMode.And,
            params Neuron[] additionalInputs
        )
            where T :
                ITruthTableStatic
                <
                    T,
                    TParam,
                    TInput,
                    TOutput,
                    TwoInputTruthTableBase.InterneuronSet
                >
        {
            bool bResult = false;
            result = default;
            if 
            (
                VariableInfo.TryParse(parameterExpression, out var variableInfo) &&
                parameters.Outputs.Parameter1 != null &&
                parameters.Outputs.Parameter2 != null &&
                parameters.Outputs.Parameter3 != null &&
                parameters.Inputs.Parameter1 != null &&
                parameters.Inputs.Parameter2 != null
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
