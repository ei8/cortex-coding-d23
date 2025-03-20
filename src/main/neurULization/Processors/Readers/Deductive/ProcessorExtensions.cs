using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    internal static class ProcessorExtensions
    {
        #region ValueReader
        internal static IExpressionParameterSet CreateInstanceValueParameterSet(
            IExternalReferenceSet externalReferences,
            Neuron value,
            Neuron greatGranny
        )
        {
            return new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        greatGranny,
                        externalReferences.Unit
                    ),
                    new UnitParameterSet(
                        value,
                        externalReferences.NominalSubject
                    )
                }
            );
        }
        #endregion

        #region ValueExpressionReader
        internal static IExpressionParameterSet CreateValueExpressionParameterSet(
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
        internal static IExpressionParameterSet CreatePropertyValueExpressionParameterSet(
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
        internal static IExpressionParameterSet CreatePropertyValueAssignmentParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyParameterSetCore propertyParameters,
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
        internal static IExpressionParameterSet CreatePropertyValueAssociationParameterSet(
            IExternalReferenceSet externalReferences,
            IPropertyParameterSetCore propertyParameters,
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
