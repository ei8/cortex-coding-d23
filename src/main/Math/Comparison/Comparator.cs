using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Comparison
{
    public partial class Comparator
    (
        Comparator.ParameterInfo parameters,
        In2TruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        In2Out3TruthTableBase
        <
            Comparator.ParameterInfo,
            In2TruthTableBase.Input,
            Comparator.Output
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
            In2TruthTableBase.Input,
            Comparator.Output,
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
