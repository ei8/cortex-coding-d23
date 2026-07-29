using ei8.Cortex.Coding.d23.Math.Logic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ei8.Cortex.Coding.d23.Collections
{
    //public class AdjacentBase : FunctionalCircuitBase
    //{
    //    public AdjacentBase() : base()
    //    {
    //    }

    //    public static bool TryCreate<T>(
    //        [NotNullWhen(true)] out T? result,
    //        FunctionalParameterInfo parameters,
    //        InterneuronTagInfo? interneuronTagInfo = null,
    //        [CallerArgumentExpression(nameof(result))] string parameterExpression = "",
    //        params Neuron[] additionalInputs
    //    ) where T : LogicGateBase, new()
    //    {
    //        bool bResult = false;
    //        result = null;
    //        if (VariableInfo.TryParse(parameterExpression, out var variableInfo))
    //        {
    //            result = new();
    //            result.Initialize(
    //                parameters,
    //                variableInfo,
    //                interneuronTagInfo,
    //                additionalInputs
    //            );
    //            bResult = true;
    //        }

    //        return bResult;
    //    }

    //    protected abstract Neuron[] GetInterneuronOutputs(BinaryNeuronInfo output);

    //    protected abstract string[] GetInterneuronTags(
    //        VariableInfo variableInfo,
    //        InterneuronTagInfo? interneuronTagInfo = null
    //    );

    //    protected static Network[] CreateInterneuronNetworks(
    //        Neuron[] outputs,
    //        string[] outputInterneuronTags
    //    ) =>
    //    [
    //        ..outputs.Select(o => {
    //        var index = Array.IndexOf(outputs, o);
    //        return NetworkHelper.CreateInterneuronNetwork(outputInterneuronTags[index], outputs[index]);
    //    })];

    //    protected abstract Network LinkInputNeurons(
    //        BinaryNeuronInfo[] inputs,
    //        params Neuron[] additionalInputs
    //    );

    //    protected override void Initialize(
    //        FunctionalParameterInfo parameters,
    //        VariableInfo variableInfo,
    //        InterneuronTagInfo? interneuronTagInfo = null,
    //        params Neuron[] additionalInputs
    //    )
    //    {
    //        this.Parameters = parameters;
    //        this.Network.AddReplaceItems(this.Parameters);
    //        this.interneurons = NetworkHelper.CreateInterneuronNetworks(
    //                this.GetInterneuronOutputs(this.Parameters.Outputs.Single()),
    //                this.GetInterneuronTags(variableInfo, interneuronTagInfo)
    //            );
    //        this.Network.AddReplaceItems(
    //            [
    //                ..this.interneurons,
    //                this.LinkInputNeurons(
    //                    parameters.Inputs,
    //                    additionalInputs
    //                )
    //            ]
    //        );
    //    }
    //}
}
