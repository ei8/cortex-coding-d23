namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public static class ProcessorExtensions
    {
        #region ValueExpressionReader
        internal static IExpressionParameterSet CreateValueExpressionParameterSet(
            IMirrorSet mirrors,
            IInductiveParameterSet parameters,
            Neuron grannyCandidate
        ) => new ExpressionParameterSet(
                parameters.Granny,
                new[] {
                    UnitParameterSet.CreateWithGrannyAndType(
                        grannyCandidate,
                        mirrors.Unit
                    ),
                }
            );
        #endregion

        #region PropertyValueExpressionReader
        internal static IExpressionParameterSet CreatePropertyValueExpressionParameterSet(
            IMirrorSet mirrors,
            IInductiveParameterSet parameters,
            Neuron grannyCandidate
        ) => new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithGrannyAndType(
                        grannyCandidate,
                        mirrors.Unit
                    ),
                    UnitParameterSet.CreateWithValueAndType(
                        mirrors.Of,
                        mirrors.Case
                    )
                }
            );
        #endregion

        #region PropertyValueAssignment
        internal static IExpressionParameterSet CreatePropertyValueAssignmentParameterSet(
            IMirrorSet mirrors,
            IPropertyParameterSet parameters,
            Neuron grannyCandidate
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        parameters.Property,
                        mirrors.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        grannyCandidate,
                        mirrors.NominalModifier
                    )
                }
            );
        #endregion

        #region PropertyValueAssociation
        internal static IExpressionParameterSet CreatePropertyValueAssociationParameterSet(
            IMirrorSet mirrors,
            IInductiveParameterSet parameters,
            Neuron grannyCandidate
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        mirrors.Has,
                        mirrors.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        grannyCandidate,
                        mirrors.DirectObject
                    )
                }
            );
        #endregion
    }
}
