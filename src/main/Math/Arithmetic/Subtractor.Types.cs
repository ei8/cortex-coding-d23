using ei8.Cortex.Coding.d23.Math.Logic;
using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Subtractor
    {
        public class ParameterInfo
        (
            Input inputs,
            Output outputs
        ) :
            FunctionalCircuitParameter
            <
                Input,
                Output
            >
            (
                inputs,
                outputs
            )
        {
        }

        public class Input(
            BinaryNeuronParameter? minuend,
            BinaryNeuronParameter? subtrahend,
            BinaryNeuronParameter? precedingBorrow
        ) :
            InputCircuitParameterSubsetBase<BinaryNeuronParameter, BinaryNeuronParameter, BinaryNeuronParameter>(
                minuend,
                subtrahend,
                precedingBorrow
            )
        {
            public BinaryNeuronParameter? Minuend => this.Parameter1;
            public BinaryNeuronParameter? Subtrahend => this.Parameter2;
            public BinaryNeuronParameter? PrecedingBorrow => this.Parameter3;
        }

        public class Output(
            BinaryNeuronParameter? difference,
            BinaryNeuronParameter? borrow
        ) :
        OutputCircuitParameterSubsetBase<BinaryNeuronParameter, BinaryNeuronParameter>(
            difference,
            borrow
        )
        {
            public BinaryNeuronParameter? Difference => this.Parameter1;
            public BinaryNeuronParameter? Borrow => this.Parameter2;
        }

        public class InterneuronSet : CircuitInterneuronSetBase
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks() =>
                NetworkHelper.ConvertToNetworks
                (
                    this.Half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend,
                    this.Half1_NOT___Minuend,
                    this.Half1_XOR___Minuend__Subtrahend,
                    this.OR___Half1_Borrow__Half2_Borrow,
                    this.Half2_AND___Borrow__Half2_OUT___Half2_NOT___Half1_XOR_Result,
                    this.Half2_NOT___Half1_XOR_Result,
                    this.Half2_XOR___Borrow__Half1_XOR_Result
                );

            public required AndGate Half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend { get; set; }
            public required NotGate Half1_NOT___Minuend { get; set; }
            public required XorGate Half1_XOR___Minuend__Subtrahend { get; set; }
            public OrGate? OR___Half1_Borrow__Half2_Borrow { get; set; }
            public AndGate? Half2_AND___Borrow__Half2_OUT___Half2_NOT___Half1_XOR_Result { get; set; }
            public NotGate? Half2_NOT___Half1_XOR_Result { get; set; }
            public XorGate? Half2_XOR___Borrow__Half1_XOR_Result { get; set; }
        }

        public class KeyedInterneuronTagger(string precedingCircuitName) : IKeyedInterneuronTagger
        {
            private readonly string precedingCircuitName = precedingCircuitName;

            public InterneuronTagInfo? CreateByKey(string key, VariableInfo variableInfo)
            {
                InterneuronTagInfo? result = null;
                var circuitName = variableInfo.Inputs.First();

                switch (key)
                {
                    case nameof(InterneuronSet.Half2_XOR___Borrow__Half1_XOR_Result):
                    case nameof(InterneuronSet.Half2_AND___Borrow__Half2_OUT___Half2_NOT___Half1_XOR_Result):
                        result =
                            new
                            (
                                circuitName,
                                this.precedingCircuitName,
                                circuitName
                            );
                        break;
                    case nameof(InterneuronSet.Half2_NOT___Half1_XOR_Result):
                    case nameof(InterneuronSet.Half1_NOT___Minuend):
                        result = InterneuronTagInfo.CreateByCommonTagPrefix(circuitName);
                        break;
                    case nameof(InterneuronSet.OR___Half1_Borrow__Half2_Borrow):
                    case nameof(InterneuronSet.Half1_XOR___Minuend__Subtrahend):
                    case nameof(InterneuronSet.Half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend):
                        result = InterneuronTagInfo.CreateByCommonTagPrefix(circuitName, 2);
                        break;
                }

                return result;
            }
        }
    }
}
