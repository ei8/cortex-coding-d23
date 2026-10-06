using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Comparison
{
    public partial class Comparator
    (
        Comparator.ParameterInfo parameters,
        TwoInputTruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        TwoInputThreeOutputTruthTableBase
        <
            Comparator.ParameterInfo,
            TwoInputTruthTableBase.Input,
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
            TwoInputTruthTableBase.InterneuronSet
        >,
        ITruthTableStatic
        <
            Comparator,
            Comparator.ParameterInfo,
            TwoInputTruthTableBase.Input,
            Comparator.Output,
            TwoInputTruthTableBase.InterneuronSet
        >
    {
        public static Comparator Create
        (
            ParameterInfo parameters, 
            TwoInputTruthTableBase.InterneuronSet interneurons, 
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
