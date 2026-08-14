using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class SequentialAdder : SequentialOperationBase<SequentialAdder.Input, SequentialAdder.Output>, ISequentialOperation<SequentialAdder, SequentialAdder.Input, SequentialAdder.Output>
    {
        public class Input(
            UnaryNeuronParameter? currentDigit,
            BinaryNeuronParameter? addend1,
            BinaryNeuronParameter? addend2,
            BinaryNeuronParameter? precedingCarryOver
        ) :
        InputCircuitParameterSubset<UnaryNeuronParameter, BinaryNeuronParameter, BinaryNeuronParameter, BinaryNeuronParameter>(
            currentDigit,
            addend1,
            addend2,
            precedingCarryOver
        )
        {
            public UnaryNeuronParameter? CurrentDigit => this.Parameter1;
            public BinaryNeuronParameter? Addend1 => this.Parameter2;
            public BinaryNeuronParameter? Addend2 => this.Parameter3;
            public BinaryNeuronParameter? PrecedingCarryOver => this.Parameter4;
        }

        public class Output(
            UnaryNeuronParameter? nextDigit,
            BinaryNeuronParameter? sum,
            BinaryNeuronParameter? carryOver
        ) :
        OutputCircuitParameterSubset<UnaryNeuronParameter, BinaryNeuronParameter, BinaryNeuronParameter>(
            nextDigit,
            sum,
            carryOver
        )
        {
            public UnaryNeuronParameter? NextDigit => this.Parameter1;
            public BinaryNeuronParameter? Sum => this.Parameter2;
            public BinaryNeuronParameter? CarryOver => this.Parameter3;
        }

        protected SequentialAdder(
            FunctionalCircuitParameter<Input, Output> parameters,
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
            FunctionalCircuitParameter<Input, Output> parameters,
            IEnumerable<ReadOnlyNetwork> networks,
            VariableInfo? variableInfo
        ) => new(
            parameters,
            networks,
            variableInfo
        );

        public static IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworks(
            FunctionalCircuitParameter<Input, Output> parameters,
            VariableInfo variableInfo,
            VariableInfo? precedingVariableInfo = null
        )
        {
            var result = new Network();

            var sum = parameters.Outputs.Sum;

            var coreNetwork = Adder.CreateInterneuronNetworksCore(
                variableInfo,
                precedingVariableInfo,
                parameters.Inputs.PrecedingCarryOver,
                parameters.Inputs.Addend1,
                parameters.Inputs.Addend2,
                sum,
                parameters.Outputs.CarryOver,
                parameters.Inputs.CurrentDigit!.Neuron
            );

            result.AddReplaceItems([..coreNetwork]);

            var nextDigit = parameters.Outputs.NextDigit;
            if (sum != null && nextDigit != null)
            {
                foreach (var sumNeuron in sum.Network.GetItems<Neuron>())
                    result.AddReplace(NetworkHelper.CreateTerminal(sumNeuron, nextDigit.Neuron));
            }

            return [result];
        }

        public static FunctionalCircuitParameter<Input, Output> GetDefaultParameters(
            UnaryNeuronParameter? currentDigit,
            BinaryNeuronParameter? input1,
            BinaryNeuronParameter? input2,
            BinaryNeuronParameter? precedingValue,
            UnaryNeuronParameter? nextDigit,
            BinaryNeuronParameter? result,
            BinaryNeuronParameter? regrouping
         )
         => new (
            new(
                currentDigit, 
                input1, 
                input2, 
                precedingValue
            ),
            new(
                nextDigit,
                result, 
                regrouping
            )
        );
    }
}
