using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class In1Out1LogicGateBase<T>
    (
        In1Out1TruthTableBase.ParameterInfo parameters,
        In1TruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        In1TruthTableBase
        <
            T,
            In1Out1TruthTableBase.ParameterInfo,
            In1TruthTableBase.Input,
            In1TruthTableBase.InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ILogicGate
        <
            In1Out1TruthTableBase.ParameterInfo,
            In1TruthTableBase.InterneuronSet
        >
        where T :
            ILogicGateStatic
            <
                T,
                In1Out1TruthTableBase.ParameterInfo,
                In1TruthTableBase.Input,
                In1Out1TruthTableBase.Output,
                In1TruthTableBase.InterneuronSet
            >
    {
        public static In1TruthTableBase.InterneuronSet CreateInterneurons
        (
            In1Out1TruthTableBase.ParameterInfo parameters,
            VariableInfo variableInfo,
            InterneuronTagInfo? interneuronTagInfo = null,
            AdditionalInputNeuronInfo? additionalInputNeurons = null
        )
        {
            var interneuronNetworks = NetworkHelper.CreateInterneuronNetworksByOutputNeurons
                (
                    T.GetOutputsPerInterneuron(parameters.Outputs),
                    T.GetInterneuronTags(variableInfo, interneuronTagInfo)
                );

            return new In1TruthTableBase.InterneuronSet
            (
                interneuronNetworks.ElementAt(0),
                interneuronNetworks.ElementAt(1),
                T.LinkInputNeurons(
                    parameters.Inputs,
                    interneuronNetworks,
                    additionalInputNeurons
                ).Combine()
            );
        }
    }
}
