using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class SequentialAdder : SequentialOperationBase, ISequentialOperation<SequentialAdder>
    {
        // TODO: Input and Output enums to classes
        public enum Input
        {
            CurrentDigit,
            Addend1,
            Addend2,
            PrecedingCarryOver
        }

        public enum Output
        {
            NextDigit,
            Sum,
            CarryOver
        }

        protected SequentialAdder(
            FunctionalParameter<NeuronParameterBase> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) : base(
            parameters,
            networks,
            variableInfo
        )
        {
        }

        public static SequentialAdder Create(
            FunctionalParameter<NeuronParameterBase> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) => new(
            parameters,
            networks,
            variableInfo
        );

        public static IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworks(
            FunctionalParameter<NeuronParameterBase> parameters,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        )
        {
            var result = new Network();

            var sum = (BinaryNeuronParameter?) parameters.Outputs.ElementAt((int)Output.Sum);

            var coreNetwork = Adder.CreateInterneuronNetworksCore(
                variableInfo,
                precedingVariableInfo,
                (BinaryNeuronParameter?)parameters.Inputs.ElementAt((int)Input.PrecedingCarryOver),
                (BinaryNeuronParameter?)parameters.Inputs.ElementAt((int)Input.Addend1),
                (BinaryNeuronParameter?)parameters.Inputs.ElementAt((int)Input.Addend2),
                sum,
                (BinaryNeuronParameter?)parameters.Outputs.ElementAt((int)Output.CarryOver),
                ((UnaryNeuronParameter?)parameters.Inputs.ElementAt((int)Input.CurrentDigit)!).Neuron
            );

            result.AddReplaceItems([..coreNetwork]);

            var nextDigit = (UnaryNeuronParameter?) parameters.Outputs.ElementAt((int)Output.NextDigit);
            if (sum != null && nextDigit != null)
            {
                foreach (var sumNeuron in sum.Network.GetItems<Neuron>())
                    result.AddReplace(NetworkHelper.CreateTerminal(sumNeuron, nextDigit.Neuron));
            }

            return [result];
        }

        public static FunctionalParameter<NeuronParameterBase> GetDefaultParameters(
            UnaryNeuronParameter? currentDigit,
            BinaryNeuronParameter? input1,
            BinaryNeuronParameter? input2,
            BinaryNeuronParameter? precedingValue,
            UnaryNeuronParameter? nextDigit,
            BinaryNeuronParameter? sum,
            BinaryNeuronParameter? carryOver
        ) => new(
            [
                currentDigit, 
                input1, 
                input2, 
                precedingValue
            ],
            [
                nextDigit,
                sum, 
                carryOver 
            ]
        );
    }
}
