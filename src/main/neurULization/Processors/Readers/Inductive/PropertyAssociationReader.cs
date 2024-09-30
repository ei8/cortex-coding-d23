using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class PropertyAssociationReader : IPropertyAssociationReader
    {
        private readonly IPropertyAssignmentReader propertyAssignmentReader;
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;

        public PropertyAssociationReader(IPropertyAssignmentReader propertyAssignmentReader, IExpressionReader expressionReader, IPrimitiveSet primitives)
        {
            this.propertyAssignmentReader = propertyAssignmentReader;
            this.expressionReader = expressionReader;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssociation>> CreateGreatGrannies(
            IPropertyAssignmentReader propertyAssignmentReader,
            IExpressionReader expressionReader,
            IPropertyAssociationParameterSet parameters,
            Ensemble ensemble,
            IPrimitiveSet primitives
        )
        {
            var ppid = parameters.Property.Tag.ToString();
            return ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssociation>(
                    expressionReader,
                    () => PropertyAssociationReader.CreateExpressionParameterSet(primitives, parameters, gc),
                    (g, r) => r.Expression = g
                )
            ).Concat(
                new IGreatGrannyInfo<IPropertyAssociation>[]
                {
                    new DependentGreatGrannyInfo<IPropertyAssignment, IPropertyAssignmentReader, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        propertyAssignmentReader,
                        g => CreatePropertyAssignmentParameterSet(
                            parameters,
                            ((IExpression) g).Units.GetValueUnitGranniesByTypeId(primitives.DirectObject.Id).Single().Value
                        ),
                        (g, r) => r.PropertyAssignment = g
                    )
                }
            );
        }

        private static ExpressionParameterSet CreateExpressionParameterSet(
            IPrimitiveSet primitives,
            IPropertyAssociationParameterSet parameters,
            Neuron unitGranny
        ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        primitives.Has,
                        primitives.Unit
                    ),
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        primitives.DirectObject
                    )
                }
            );

        private static IPropertyAssignmentParameterSet CreatePropertyAssignmentParameterSet(IPropertyAssociationParameterSet parameters, Neuron value) =>
            new PropertyAssignmentParameterSet(
                value,
                parameters.Property,
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, IPropertyAssociationParameterSet parameters, out IPropertyAssociation result) =>
            new PropertyAssociation().AggregateTryParse(
                parameters.Granny,
                PropertyAssociationReader.CreateGreatGrannies(
                    this.propertyAssignmentReader,
                    this.expressionReader,
                    parameters,
                    ensemble,
                    this.primitives
                ),
                new IGreatGrannyProcess<IPropertyAssociation>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssociation>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IPropertyAssignment, IPropertyAssignmentReader, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                2,
                out result
            );
    }
}
