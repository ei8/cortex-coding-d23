using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    internal static class ProcessorExtensions
    {
        #region ValueReader
        internal static IIdExpressionParameterSet CreateIdInstanceValueParameterSet(
            IMirrorSet mirrors,
            Neuron value,
            Neuron greatGranny,
            Guid id
        ) => new IdExpressionParameterSet(
            new[]
            {
                new UnitParameterSet(
                    greatGranny,
                    mirrors.Unit
                ),
                new UnitParameterSet(
                    value,
                    mirrors.NominalSubject
                )
            },
            id
        );

        internal static IExpressionParameterSet CreateInstanceValueParameterSet(
            IMirrorSet mirrors,
            Neuron value,
            Neuron greatGranny
        ) => new ExpressionParameterSet(
            new[]
            {
                new UnitParameterSet(
                    greatGranny,
                    mirrors.Unit
                ),
                new UnitParameterSet(
                    value,
                    mirrors.NominalSubject
                )
            }
        );
        #endregion

        #region ValueExpressionReader
        internal static IExpressionParameterSet CreateValueExpressionParameterSet(
            IMirrorSet mirrors,
            Neuron greatGranny
        ) => new ExpressionParameterSet(
            new[] {
                new UnitParameterSet(
                    greatGranny,
                    mirrors.Unit
                ),
            }
        );
        #endregion

        #region PropertyValueExpressionReader
        internal static IExpressionParameterSet CreatePropertyValueExpressionParameterSet(
            IMirrorSet mirrors,
            Neuron greatGranny
        ) => new ExpressionParameterSet(
            new[]
            {
                new UnitParameterSet(
                    greatGranny,
                    mirrors.Unit
                ),
                new UnitParameterSet(
                    mirrors.Of,
                    mirrors.Case
                )
            }
        );
        #endregion

        #region PropertyValueAssignmentReader
        internal static IExpressionParameterSet CreatePropertyValueAssignmentParameterSet(
            IMirrorSet mirrors,
            IPropertyParameterSetCore propertyParameters,
            Neuron greatGranny
        ) => new ExpressionParameterSet(
            new[]
            {
                new UnitParameterSet(
                    propertyParameters.Property,
                    mirrors.Unit
                ),
                new UnitParameterSet(
                    greatGranny,
                    mirrors.NominalModifier
                )
            }
        );
        #endregion

        #region PropertyValueAssociationReader
        internal static IExpressionParameterSet CreatePropertyValueAssociationParameterSet(
            IMirrorSet mirrors,
            Neuron greatGranny
        ) => new ExpressionParameterSet(
            new[]
            {
                new UnitParameterSet(
                    mirrors.Has,
                    mirrors.Unit
                ),
                new UnitParameterSet(
                    greatGranny,
                    mirrors.DirectObject
                )
            }
        );
        #endregion
    }
}
