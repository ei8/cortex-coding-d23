using ei8.Cortex.Coding.d23.Math.Logic;
using ei8.Cortex.Coding.d23.Math.TruthTables;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Adder
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

        public class Input
        (
            BinaryNeuronParameter? addend1,
            BinaryNeuronParameter? addend2,
            BinaryNeuronParameter? precedingCarryOver
        ) :
            InputCircuitParameterSubsetBase
            <
                BinaryNeuronParameter, 
                BinaryNeuronParameter, 
                BinaryNeuronParameter
            >
            (
                addend1,
                addend2,
                precedingCarryOver
            )
        {
            public BinaryNeuronParameter? Addend1 => this.Parameter1;
            public BinaryNeuronParameter? Addend2 => this.Parameter2;
            public BinaryNeuronParameter? PrecedingCarryOver => this.Parameter3;
        }

        public class Output
        (
            BinaryNeuronParameter? sum,
            BinaryNeuronParameter? carryOver
        ) :
            OutputCircuitParameterSubsetBase
            <
                BinaryNeuronParameter, 
                BinaryNeuronParameter
            >
            (
                sum,
                carryOver
            )
        {
            public BinaryNeuronParameter? Sum => this.Parameter1;
            public BinaryNeuronParameter? CarryOver => this.Parameter2;
        }

        public class InterneuronSet : CircuitInterneuronSetBase
        {
            protected override IEnumerable<ReadOnlyNetwork> GetNetworks()
            {
                IEnumerable<ReadOnlyNetwork?> nets = 
                    [
                        this.Half1_AND___Addend1__Addend2?.Network,
                        this.Half1_XOR___Addend1__Addend2?.Network,
                        this.Half2_AND___CarryOver__Half1_XOR_Result?.Network,
                        this.Half2_XOR___CarryOver__Half1_XOR_Result?.Network,
                        this.OR___Half1_CarryOver__Half2_CarryOver?.Network
                    ];

                return nets.WhereNotNull();
            }

            public AndGate? Half1_AND___Addend1__Addend2 { get; internal set; }
            public XorGate? Half1_XOR___Addend1__Addend2 { get; internal set; }
            public AndGate? Half2_AND___CarryOver__Half1_XOR_Result { get; internal set; }
            public XorGate? Half2_XOR___CarryOver__Half1_XOR_Result { get; internal set; }
            public OrGate? OR___Half1_CarryOver__Half2_CarryOver { get; internal set; }
        }

        public class KeyedInterneuronTagger(string precedingCircuitName) : IKeyedInterneuronTagger
        {
            public KeyedInterneuronTagger() : this(string.Empty)
            {                
            }

            private readonly string precedingCircuitName = precedingCircuitName;

            public InterneuronTagInfo? CreateByKey(string key, VariableInfo variableInfo)
            {
                InterneuronTagInfo? result = null;
                var circuitName = variableInfo.Inputs.First();

                switch (key)
                {
                    case nameof(InterneuronSet.Half2_XOR___CarryOver__Half1_XOR_Result):
                    case nameof(InterneuronSet.Half2_AND___CarryOver__Half1_XOR_Result):
                        result = 
                            new
                            (
                                circuitName,
                                this.precedingCircuitName,
                                circuitName
                            );
                        break;
                    case nameof(InterneuronSet.OR___Half1_CarryOver__Half2_CarryOver):
                    case nameof(InterneuronSet.Half1_XOR___Addend1__Addend2):
                    case nameof(InterneuronSet.Half1_AND___Addend1__Addend2):
                        result = InterneuronTagInfo.CreateByCommonTagPrefix(circuitName, 2);
                        break;
                }

                return result;
            }
        }
    }
}
