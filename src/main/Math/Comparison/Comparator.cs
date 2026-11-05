using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Comparison
{
    public partial class Comparator
    (
        Comparator.ParameterInfo parameters,
        In2TruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        In2TruthTableBase
        <
            Comparator,
            Comparator.ParameterInfo,
            In2TruthTableBase.Input,
            In2TruthTableBase.InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        IComparison
        <
            Comparator.ParameterInfo,
            In2TruthTableBase.InterneuronSet
        >,
        ITruthTableStatic
        <
            Comparator,
            Comparator.ParameterInfo,
            In2TruthTableBase.InterneuronSet
        >
    {
        public static Comparator Create
        (
            ParameterInfo parameters, 
            In2TruthTableBase.InterneuronSet interneurons, 
            VariableInfo? variableInfo
        ) =>
            new
            (
                parameters,
                interneurons,
                variableInfo
            );
        // TODO: Unify with In2Out1LogicGateBase.CreateInterneurons
        public static In2TruthTableBase.InterneuronSet CreateInterneurons
        (
            ParameterInfo parameters, 
            VariableInfo variableInfo, 
            InterneuronTagInfo? interneuronTagInfo = null, 
            AdditionalInputNeuronInfo? additionalInputNeurons = null
        )
        {
            var interneuronNetworks = NetworkHelper.CreateInterneuronNetworksByOutputNeurons
                (
                    Comparator.GetOutputsPerInterneuron(parameters.Outputs),
                    Comparator.GetInterneuronTags(variableInfo, interneuronTagInfo)
                );

            return new In2TruthTableBase.InterneuronSet
            (
                interneuronNetworks.ElementAt(0),
                interneuronNetworks.ElementAt(1),
                interneuronNetworks.ElementAt(2),
                interneuronNetworks.ElementAt(3),
                Comparator.LinkInputNeurons
                (
                    parameters.Inputs,
                    interneuronNetworks,
                    additionalInputNeurons
                ).Combine()
            );
        }

        public static IEnumerable<IEnumerable<Neuron>> GetOutputsPerInterneuron(Comparator.Output outputs) =>
            outputs.AreEqual != null &&
            outputs.IsLessThan != null &&
            outputs.IsGreaterThan != null ?
                [
                    [
                        outputs.AreEqual.Neuron1,
                        outputs.IsLessThan.Neuron0,
                        outputs.IsGreaterThan.Neuron0
                    ],
                    [
                        outputs.AreEqual.Neuron0,
                        outputs.IsLessThan.Neuron1,
                        outputs.IsGreaterThan.Neuron0
                    ],
                    [
                        outputs.AreEqual.Neuron0,
                        outputs.IsLessThan.Neuron0,
                        outputs.IsGreaterThan.Neuron1
                    ],
                    [
                        outputs.AreEqual.Neuron1,
                        outputs.IsLessThan.Neuron0,
                        outputs.IsGreaterThan.Neuron0
                    ]
                ] :
                [];
    }
}
