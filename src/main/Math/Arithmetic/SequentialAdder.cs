using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public class SequentialAdder :
        SequentialOperationBase
        <
            SequentialAdder.Input,
            SequentialAdder.Output,
            InterneuronSet
        >, 
        ISequentialOperation
        <
            SequentialAdder, 
            SequentialAdder.Input,
            SequentialAdder.Output,
            InterneuronSet
        >
    {
        public class Input
        (
            IEnumerable<UnaryNeuronParameter?> currentDigits,
            BinaryNeuronParameter? addend1,
            BinaryNeuronParameter? addend2,
            BinaryNeuronParameter? precedingCarryOver = null
        ) : 
            CircuitParameterSubsetBase, 
            IInputCircuitParameterSubset
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() => 
                NetworkHelper.ConvertToNetworks
                (
                    [
                        ..this.CurrentDigits,
                        this.Addend1,
                        this.Addend2,
                        this.PrecedingCarryOver
                    ]
                );

            public IEnumerable<UnaryNeuronParameter?> CurrentDigits { get; } = currentDigits;
            public BinaryNeuronParameter? Addend1 { get; } = addend1;
            public BinaryNeuronParameter? Addend2 { get; } = addend2;
            public BinaryNeuronParameter? PrecedingCarryOver { get; } = precedingCarryOver;
        }

        public class Output
        (
            IEnumerable<UnaryNeuronParameter?> nextDigits,
            BinaryNeuronParameter? sum,
            BinaryNeuronParameter? carryOver
        ) : 
            CircuitParameterSubsetBase, 
            IOutputCircuitParameterSubset
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
                NetworkHelper.ConvertToNetworks
                (
                    [
                        ..this.NextDigits,
                        this.Sum,
                        this.CarryOver
                    ]
                );

            public IEnumerable<UnaryNeuronParameter?> NextDigits { get; } = nextDigits;
            public BinaryNeuronParameter? Sum { get; } = sum;
            public BinaryNeuronParameter? CarryOver { get; } = carryOver;
        }

        protected SequentialAdder(
            FunctionalCircuitParameter<Input, Output> parameters,
            InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) : base(
            parameters,
            interneurons,
            variableInfo
        )
        {
        }

        public static SequentialAdder Create(
            FunctionalCircuitParameter<Input, Output> parameters,
            InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => new(
            parameters,
            interneurons,
            variableInfo
        );

        public static InterneuronSet CreateInterneurons(
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

            result.AddReplaceItems(coreNetwork.Network);

            var nextDigits = parameters.Outputs.NextDigits;
            if (sum != null)
            {
                foreach (var sumNeuron in sum.Network.GetItems<Neuron>())
                    foreach (var nextDigit in nextDigits.WhereNotNull())
                        result.AddReplace(NetworkHelper.CreateTerminal(sumNeuron, nextDigit.Neuron));
            }

            // TODO: Create InterneuronSet specifically for SequentialAdder
            return new InterneuronSet([result]);
        }
    }
}
