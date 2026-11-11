using ei8.Cortex.Coding.d23.Math.Logic;
using ei8.Cortex.Coding.d23.Math.TruthTables;
using System;
using System.Linq;

namespace ei8.Cortex.Coding.d23.Math.Arithmetic
{
    public partial class Subtractor
    (
        Subtractor.ParameterInfo parameters,
        Subtractor.InterneuronSet interneurons,
        VariableInfo? variableInfo
    ) : 
        TruthTableBase
        <
            Subtractor,
            Subtractor.ParameterInfo,
            Subtractor.Input,
            Subtractor.Output,
            Subtractor.InterneuronSet,
            Subtractor.KeyedInterneuronTagger
        >
        (
            parameters,
            interneurons,
            variableInfo
        ),
        ITruthTableStatic
        <
            Subtractor,
            Subtractor.ParameterInfo,
            Subtractor.Input,
            Subtractor.Output,
            Subtractor.InterneuronSet,
            Subtractor.KeyedInterneuronTagger
        >,
        IGroupedOperationStatic
        <
            Subtractor, 
            Subtractor.ParameterInfo, 
            Subtractor.InterneuronSet
        >
    {
        public static Subtractor Create(
            Subtractor.ParameterInfo parameters,
            InterneuronSet interneurons,
            VariableInfo? variableInfo
        ) => 
            new
            (
                parameters, 
                interneurons, 
                variableInfo
            );

        public static Subtractor.ParameterInfo CreateDefaultParameters
        (
            BinaryNeuronParameter? precedingValue,
            int exponent
        ) => 
            new
            (
                new
                (
                    BinaryNeuronParameter.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Input.Minuend)}"),
                    BinaryNeuronParameter.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Input.Subtrahend)}"),
                    precedingValue
                ),
                new
                (
                    BinaryNeuronParameter.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Output.Difference)}"),
                    BinaryNeuronParameter.Create($"{nameof(Subtractor)}{exponent + 1}.{nameof(Output.Borrow)}")
                )
            );

        public static InterneuronSet CreateInterneurons
        (
            ParameterInfo parameters, 
            VariableInfo variableInfo, 
            KeyedInterneuronTagger? interneuronTagger = null, 
            AdditionalInputNeuronInfo? additionalInputNeurons = null
        )
        {
            ArgumentNullException.ThrowIfNull(interneuronTagger);

            InterneuronSet? result = null;
            string subtractorName = variableInfo.Inputs.First();
           
            // Declare Outputs
            if (BinaryNeuronParameter.TryCreate(out var half1_OUT___Half1_NOT___Minuend, subtractorName))
            {
                if 
                (
                    // is not least significant bit
                    parameters.Inputs.PrecedingBorrow != null &&
                    BinaryNeuronParameter.TryCreate(out var half1_XOR_Result, subtractorName) &&
                    BinaryNeuronParameter.TryCreate(out var half2_OUT___Half2_NOT___Half1_XOR_Result, subtractorName) &&
                    BinaryNeuronParameter.TryCreate(out var half1_Borrow, subtractorName) &&
                    BinaryNeuronParameter.TryCreate(out var half2_Borrow, subtractorName)
                )
                {
                    // half1 interneurons
                    Subtractor.CreateSubtractorHalf1Interneurons(
                        variableInfo,
                        parameters.Inputs.Minuend,
                        parameters.Inputs.Subtrahend,
                        half1_XOR_Result,
                        half1_OUT___Half1_NOT___Minuend,
                        half1_Borrow,
                        interneuronTagger,
                        out var half1_XOR___Minuend__Subtrahend,
                        out var half1_NOT___Minuend,
                        out var half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend
                    );

                    // half2 interneurons
                    if (
                        XorGate.TryCreate(
                            out var half2_XOR___Borrow__Half1_XOR_Result,
                            new(
                                new(
                                    parameters.Inputs.PrecedingBorrow,
                                    half1_XOR_Result
                                ),
                                new(
                                    parameters.Outputs.Difference
                                )
                            ),
                            new
                            (
                                interneuronTagger.CreateByKey
                                (
                                    nameof(InterneuronSet.Half2_XOR___Borrow__Half1_XOR_Result),
                                    variableInfo
                                )
                            )
                        ) &&
                        NotGate.TryCreate(
                            out var half2_NOT___Half1_XOR_Result,
                            new(
                                new(
                                    half1_XOR_Result
                                ),
                                new(
                                    half2_OUT___Half2_NOT___Half1_XOR_Result
                                )
                            ),
                            new
                            (
                                interneuronTagger.CreateByKey
                                (
                                    nameof(InterneuronSet.Half2_NOT___Half1_XOR_Result),
                                    variableInfo
                                )
                            )
                        ) &&
                        AndGate.TryCreate(
                            out var half2_AND___Borrow__Half2_OUT___Half2_NOT___Half1_XOR_Result,
                            new(
                                new(
                                    parameters.Inputs.PrecedingBorrow,
                                    half2_OUT___Half2_NOT___Half1_XOR_Result
                                ),
                                new(
                                    half2_Borrow
                                )
                            ),
                            new
                            (
                                interneuronTagger.CreateByKey
                                (
                                    nameof(InterneuronSet.Half2_AND___Borrow__Half2_OUT___Half2_NOT___Half1_XOR_Result),
                                    variableInfo
                                )
                            )
                        ) &&
                        // OR Borrows
                        OrGate.TryCreate(
                            out var OR___Half1_Borrow__Half2_Borrow,
                            new(
                                new(
                                    half1_Borrow,
                                    half2_Borrow
                                ),
                                new(
                                    parameters.Outputs.Borrow
                                )
                            ),
                            new
                            (
                                interneuronTagger.CreateByKey
                                (
                                    nameof(InterneuronSet.OR___Half1_Borrow__Half2_Borrow),
                                    variableInfo
                                )
                            )
                        )
                    )
                    {
                        result = new InterneuronSet()
                        {
                            Half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend = half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend,
                            Half1_NOT___Minuend = half1_NOT___Minuend,
                            Half1_XOR___Minuend__Subtrahend = half1_XOR___Minuend__Subtrahend,
                            Half2_XOR___Borrow__Half1_XOR_Result = half2_XOR___Borrow__Half1_XOR_Result,
                            Half2_NOT___Half1_XOR_Result = half2_NOT___Half1_XOR_Result,
                            Half2_AND___Borrow__Half2_OUT___Half2_NOT___Half1_XOR_Result = half2_AND___Borrow__Half2_OUT___Half2_NOT___Half1_XOR_Result,
                            OR___Half1_Borrow__Half2_Borrow = OR___Half1_Borrow__Half2_Borrow
                        };                        
                    }
                }
                else
                {
                    // half1
                    Subtractor.CreateSubtractorHalf1Interneurons
                    (
                        variableInfo,
                        parameters.Inputs.Minuend,
                        parameters.Inputs.Subtrahend,
                        parameters.Outputs.Difference,
                        half1_OUT___Half1_NOT___Minuend,
                        parameters.Outputs.Borrow,
                        interneuronTagger,
                        out var half1_XOR___Minuend__Subtrahend,
                        out var half1_NOT___Minuend,
                        out var half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend
                    );

                    result = new InterneuronSet()
                    {
                        Half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend = half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend,
                        Half1_NOT___Minuend = half1_NOT___Minuend,
                        Half1_XOR___Minuend__Subtrahend = half1_XOR___Minuend__Subtrahend
                    };
                }
            }

            ArgumentNullException.ThrowIfNull(result);

            return result;
        }

        private static void CreateSubtractorHalf1Interneurons
        (
            VariableInfo variableInfo,
            BinaryNeuronParameter? minuend,
            BinaryNeuronParameter? subtrahend,
            BinaryNeuronParameter? xorOutput,
            BinaryNeuronParameter? notOutput,
            BinaryNeuronParameter? andOutput,
            KeyedInterneuronTagger interneuronTagger,
            out XorGate half1_XOR___Minuend__Subtrahend,
            out NotGate half1_NOT___Minuend,
            out AndGate half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend
        )
        {
            // Link half1 interneurons
            if
            (
                !(
                    XorGate.TryCreate
                    (
                        out half1_XOR___Minuend__Subtrahend!,
                        new(
                            new(
                                minuend,
                                subtrahend
                            ),
                            new(
                                xorOutput
                            )
                        ),
                        new
                        (
                            interneuronTagger.CreateByKey
                            (
                                nameof(InterneuronSet.Half1_XOR___Minuend__Subtrahend),
                                variableInfo
                            )
                        )
                    ) &&
                    NotGate.TryCreate
                    (
                        out half1_NOT___Minuend!,
                        new(
                            new(
                                minuend
                            ),
                            new(
                                notOutput
                            )
                        ),
                        new
                        (
                            interneuronTagger.CreateByKey
                            (
                                nameof(InterneuronSet.Half1_NOT___Minuend),
                                variableInfo
                            )
                        )
                    ) &&
                    AndGate.TryCreate
                    (
                        out half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend!,
                        new(
                            new(
                                notOutput,
                                subtrahend
                            ),
                            new(
                                andOutput
                            )
                        ),
                        new
                        (
                            interneuronTagger.CreateByKey
                            (
                                nameof(InterneuronSet.Half1_AND___Subtrahend__Half1_OUT___Half1_NOT___Minuend),
                                variableInfo
                            )
                        )
                    )
                )
            )
                throw new InvalidOperationException();
        }
    }
}
