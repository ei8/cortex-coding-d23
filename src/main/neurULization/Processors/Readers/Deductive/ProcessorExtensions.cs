using System;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public static class ProcessorExtensions
    {
        #region ValueReader
        public static IIdExpressionParameterSet CreateIdInstanceValueParameterSet(
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

        public static IExpressionParameterSet CreateInstanceValueParameterSet(
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
        /// <summary>
        /// Creates a ValueExpressionParameterSet which consists of a "Simple" expression 
        /// that contains a single syntactic unit which is a merge of a Value and the Unit neuron, making it a Head unit.
        /// </summary>
        /// <param name="mirrors"></param>
        /// <param name="greatGranny"></param>
        /// <returns></returns>
        public static IExpressionParameterSet CreateValueExpressionParameterSet(
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
        public static IExpressionParameterSet CreatePropertyValueExpressionParameterSet(
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
        public static IExpressionParameterSet CreatePropertyValueAssignmentParameterSet(
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
        public static IExpressionParameterSet CreatePropertyValueAssociationParameterSet(
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
