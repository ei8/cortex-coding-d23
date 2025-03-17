namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    internal static class ProcessorExtensions
    {
        #region ValueExpressionReader
        internal static ExpressionParameterSet CreateValueExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            Neuron greatGranny
        ) => new ExpressionParameterSet(
            new[] {
                new UnitParameterSet(
                    greatGranny,
                    externalReferences.Unit
                ),
            }
        );
        #endregion

        #region PropertyValueExpressionReader
        internal static ExpressionParameterSet CreatePropertyValueExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            Neuron greatGranny
        ) => new ExpressionParameterSet(
            new[]
            {
                new UnitParameterSet(
                    greatGranny,
                    externalReferences.Unit
                ),
                new UnitParameterSet(
                    externalReferences.Of,
                    externalReferences.Case
                )
            }
        );
        #endregion

        #region PropertyValueAssignmentReader
        internal static ExpressionParameterSet CreatePropertyValueAssignmentParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyParameterSet propertyParameters,
            Neuron greatGranny
        ) => new ExpressionParameterSet(
            new[]
            {
                new UnitParameterSet(
                    propertyParameters.Property,
                    externalReferences.Unit
                ),
                new UnitParameterSet(
                    greatGranny,
                    externalReferences.NominalModifier
                )
            }
        );
        #endregion

        #region PropertyValueAssociationReader
        internal static ExpressionParameterSet CreatePropertyValueAssociationParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyParameterSet propertyParameters,
            Neuron greatGranny
        ) => new ExpressionParameterSet(
            new[]
            {
                new UnitParameterSet(
                    externalReferences.Has,
                    externalReferences.Unit
                ),
                new UnitParameterSet(
                    greatGranny,
                    externalReferences.DirectObject
                )
            }
        );
        #endregion
    }
}
