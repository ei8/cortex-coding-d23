using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyAssociationReader : IPropertyAssociationReader
    {
        private readonly IPropertyAssignmentReader propertyAssignmentReader;
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;

        public PropertyAssociationReader(
            IPropertyAssignmentReader propertyAssignmentReader, 
            IExpressionReader expressionReader, 
            IPrimitiveSet primitives
        )
        {
            this.propertyAssignmentReader = propertyAssignmentReader;
            this.expressionReader = expressionReader;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssociation>> CreateGreatGrannies(
            IPropertyAssignmentReader propertyAssignmentReader,
            IExpressionReader expressionReader,
            IPropertyAssociationParameterSet parameters,
            IPrimitiveSet primitives
        ) =>
          new IGreatGrannyInfo<IPropertyAssociation>[]
          {
              new IndependentGreatGrannyInfo<IPropertyAssignment, IPropertyAssignmentReader, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                    propertyAssignmentReader,
                    () => PropertyAssociationReader.CreatePropertyAssignmentParameterSet(parameters),
                    (g, r) => r.PropertyAssignment = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssociation>(
                    expressionReader,
                    (g) => PropertyAssociationReader.CreateExpressionParameterSet(primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
          };

        private static IPropertyAssignmentParameterSet CreatePropertyAssignmentParameterSet(IPropertyAssociationParameterSet parameters) =>
            new PropertyAssignmentParameterSet(
                parameters.Property,
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(IPrimitiveSet primitives, IPropertyAssociationParameterSet parameters, Neuron neuron) =>
            new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        primitives.Has,
                        primitives.Unit
                    ),
                    new UnitParameterSet(
                        neuron,
                        primitives.DirectObject
                    )
                }
            );

        public IEnumerable<IGrannyQuery> GetQueries(IPropertyAssociationParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IPropertyAssignment, IPropertyAssignmentReader, IPropertyAssignmentParameterSet>(
                    this.propertyAssignmentReader,
                    (n) => CreatePropertyAssignmentParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionReader, IExpressionParameterSet>(
                    this.expressionReader,
                    (n) => CreateExpressionParameterSet(this.primitives, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Ensemble ensemble, IPropertyAssociationParameterSet parameters, out IPropertyAssociation result) =>
            new PropertyAssociation().AggregateTryParse(
                PropertyAssociationReader.CreateGreatGrannies(
                    this.propertyAssignmentReader,
                    this.expressionReader,
                    parameters,
                    this.primitives
                ),
                new IGreatGrannyProcess<IPropertyAssociation>[]
                {
                    new GreatGrannyProcess<IPropertyAssignment, IPropertyAssignmentReader, IPropertyAssignmentParameterSet, IPropertyAssociation>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssociation>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                out result
            );
    }
}
