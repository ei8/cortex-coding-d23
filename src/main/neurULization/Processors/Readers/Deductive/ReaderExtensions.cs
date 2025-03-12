namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    internal static class ReaderExtensions
    {
        #region ValueExpressionReader
        internal static ExpressionParameterSet CreateExpressionParameterSet(
            this IInstanceValueExpressionReader reader,
            IExternalReferenceSet externalReferences,
            Neuron greatGranny
        ) => ReaderExtensions.CreateValueExpressionReaderExpressionParameterSet(
            externalReferences,
            greatGranny
        );

        internal static ExpressionParameterSet CreateExpressionParameterSet(
            this IValueExpressionReader reader,
            IExternalReferenceSet externalReferences,
            Neuron greatGranny
        ) => ReaderExtensions.CreateValueExpressionReaderExpressionParameterSet(
            externalReferences,
            greatGranny
        );

        private static ExpressionParameterSet CreateValueExpressionReaderExpressionParameterSet(
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
        internal static ExpressionParameterSet CreateExpressionParameterSet(
            this IPropertyInstanceValueExpressionReader reader,
            IExternalReferenceSet externalReferences,
            IPropertyParameterSet propertyParameters,
            Neuron greatGranny
        ) => ReaderExtensions.CreatePropertyValueExpressionReaderExpressionParameterSet(
            externalReferences,
            propertyParameters,
            greatGranny
        );

        internal static ExpressionParameterSet CreateExpressionParameterSet(
            this IPropertyValueExpressionReader reader,
            IExternalReferenceSet externalReferences,
            IPropertyParameterSet propertyParameters,
            Neuron greatGranny
        ) => ReaderExtensions.CreatePropertyValueExpressionReaderExpressionParameterSet(
            externalReferences,
            propertyParameters,
            greatGranny
        );

        private static ExpressionParameterSet CreatePropertyValueExpressionReaderExpressionParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyParameterSet propertyParameters,
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
        internal static ExpressionParameterSet CreateExpressionParameterSet(
            this IPropertyInstanceValueAssignmentReader reader,
            IExternalReferenceSet externalReferences,
            IPropertyParameterSet propertyParameters,
            Neuron greatGranny
        ) => ReaderExtensions.CreatePropertyValueAssignmentReaderExpressionParameterSet(
            externalReferences,
            propertyParameters,
            greatGranny
        );

        internal static ExpressionParameterSet CreateExpressionParameterSet(
            this IPropertyValueAssignmentReader reader,
            IExternalReferenceSet externalReferences,
            IPropertyParameterSet propertyParameters,
            Neuron greatGranny
        ) => ReaderExtensions.CreatePropertyValueAssignmentReaderExpressionParameterSet(
            externalReferences,
            propertyParameters,
            greatGranny
        );

        private static ExpressionParameterSet CreatePropertyValueAssignmentReaderExpressionParameterSet(
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
        internal static ExpressionParameterSet CreateExpressionParameterSet(
            this IPropertyInstanceValueAssociationReader reader,
            IExternalReferenceSet externalReferences,
            IPropertyParameterSet propertyParameters,
            Neuron greatGranny
        ) => ReaderExtensions.CreatePropertyValueAssociationReaderExpressionParameterSet(
            externalReferences,
            propertyParameters,
            greatGranny
        );

        internal static ExpressionParameterSet CreateExpressionParameterSet(
            this IPropertyValueAssociationReader reader,
            IExternalReferenceSet externalReferences,
            IPropertyParameterSet propertyParameters,
            Neuron greatGranny
        ) => ReaderExtensions.CreatePropertyValueAssociationReaderExpressionParameterSet(
            externalReferences,
            propertyParameters,
            greatGranny
        );

        private static ExpressionParameterSet CreatePropertyValueAssociationReaderExpressionParameterSet(
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
