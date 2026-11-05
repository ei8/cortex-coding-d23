using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Logic
{
    public abstract class In2Out1LogicGateBase<T>
    (
        In2Out1TruthTableBase.ParameterInfo parameters,
        In2TruthTableBase.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) :
        In2TruthTableBase
        <
            T,
            In2Out1TruthTableBase.ParameterInfo,
            In2TruthTableBase.Input,
            In2TruthTableBase.InterneuronSet
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ILogicGate
        <
            In2Out1TruthTableBase.ParameterInfo,
            In2TruthTableBase.InterneuronSet
        >
        where T : 
            ILogicGateStatic
            <
                T,
                In2Out1TruthTableBase.ParameterInfo,
                In2TruthTableBase.Input,
                In2Out1TruthTableBase.Output,
                In2TruthTableBase.InterneuronSet
            >
    {
        // TODO: Unify with Comparator.CreateInterneurons
        public static In2TruthTableBase.InterneuronSet CreateInterneurons
        (
            In2Out1TruthTableBase.ParameterInfo parameters, 
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

            return new In2TruthTableBase.InterneuronSet
            (
                interneuronNetworks.ElementAt(0),
                interneuronNetworks.ElementAt(1),
                interneuronNetworks.ElementAt(2),
                interneuronNetworks.ElementAt(3),
                T.LinkInputNeurons
                (
                    parameters.Inputs,
                    interneuronNetworks,
                    additionalInputNeurons
                ).Combine()
            );
        }
    }
}
