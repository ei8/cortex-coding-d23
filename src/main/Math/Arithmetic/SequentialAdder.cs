using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class SequentialAdder :
        SequentialOperationBase
        <
            SequentialAdder.Input,
            SequentialAdder.Output
        >, 
        ISequentialOperation
        <
            SequentialAdder, 
            SequentialAdder.Input,
            SequentialAdder.Output
        >
    {
        public class Input : 
            CircuitParameterSubsetBase, 
            IInputCircuitParameterSubset
        {
            public Input
            (
                IEnumerable<UnaryNeuronParameter?> currentDigits,
                BinaryNeuronParameter? addend1,
                BinaryNeuronParameter? addend2,
                BinaryNeuronParameter? precedingCarryOver = null
            )
            {
                base.AddReplace
                (
                    [
                        .. this.CurrentDigits = currentDigits,
                        this.Addend1 = addend1,
                        this.Addend2 = addend2,
                        this.PrecedingCarryOver = precedingCarryOver
                    ]
                );
            }

            public IEnumerable<UnaryNeuronParameter?> CurrentDigits { get; }
            public BinaryNeuronParameter? Addend1 { get; }
            public BinaryNeuronParameter? Addend2 { get; }
            public BinaryNeuronParameter? PrecedingCarryOver { get; }
        }

        public class Output : CircuitParameterSubsetBase, IOutputCircuitParameterSubset
        {
            public Output
            (
                IEnumerable<UnaryNeuronParameter?> nextDigits,
                BinaryNeuronParameter? sum,
                BinaryNeuronParameter? carryOver
            )
            {
                base.AddReplace
                (
                    [
                        .. this.NextDigits = nextDigits,
                        this.Sum = sum,
                        this.CarryOver = carryOver
                    ]
                );
            }
            public IEnumerable<UnaryNeuronParameter?> NextDigits { get; }
            public BinaryNeuronParameter? Sum { get; }
            public BinaryNeuronParameter? CarryOver { get; }
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

            var coreNetwork = Adder.CreateInterneuronNetworksCore
            (
                variableInfo,
                precedingVariableInfo,
                parameters.Inputs.PrecedingCarryOver,
                parameters.Inputs.Addend1,
                parameters.Inputs.Addend2,
                sum,
                parameters.Outputs.CarryOver,
                NetworkHelper.AdditionalInputNeuronType.Or,
                [..parameters.Inputs.CurrentDigits.WhereNotNull().Select(cd => cd.Neuron)]
            );

            result.AddReplaceItems([..coreNetwork]);

            var nextDigits = parameters.Outputs.NextDigits;
            if (sum != null)
            {
                foreach (var sumNeuron in sum.Network.GetItems<Neuron>())
                    foreach (var nextDigit in nextDigits.WhereNotNull())
                        result.AddReplace(NetworkHelper.CreateTerminal(sumNeuron, nextDigit.Neuron));
            }

            return [result];
        }
    }
}
