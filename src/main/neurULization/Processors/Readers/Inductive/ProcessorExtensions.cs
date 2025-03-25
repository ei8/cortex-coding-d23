namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public static class ProcessorExtensions
    {
        #region ValueExpressionReader
        internal static IExpressionParameterSet CreateValueExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IInductiveParameterSet parameters,
            Neuron grannyCandidate
        ) => new ExpressionParameterSet(
                parameters.Granny,
                new[] {
                    UnitParameterSet.CreateWithGrannyAndType(
                        grannyCandidate,
                        externalReferences.Unit
                    ),
                }
            );
        #endregion

        #region PropertyValueExpressionReader
        internal static IExpressionParameterSet CreatePropertyValueExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IInductiveParameterSet parameters,
            Neuron grannyCandidate
        ) => new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithGrannyAndType(
                        grannyCandidate,
                        externalReferences.Unit
                    ),
                    UnitParameterSet.CreateWithValueAndType(
                        externalReferences.Of,
                        externalReferences.Case
                    )
                }
            );
        #endregion

        #region PropertyValueAssignment
        internal static IExpressionParameterSet CreatePropertyValueAssignmentParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyParameterSet parameters,
            Neuron grannyCandidate
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        parameters.Property,
                        externalReferences.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        grannyCandidate,
                        externalReferences.NominalModifier
                    )
                }
            );
        #endregion

        #region PropertyValueAssociation
        internal static IExpressionParameterSet CreatePropertyValueAssociationParameterSet(
            IExternalReferenceSet externalReferences,
            IInductiveParameterSet parameters,
            Neuron grannyCandidate
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        externalReferences.Has,
                        externalReferences.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        grannyCandidate,
                        externalReferences.DirectObject
                    )
                }
            );
        #endregion
    }
}
