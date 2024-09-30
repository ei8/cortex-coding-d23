using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class PropertyAssignmentWriter : IPropertyAssignmentWriter
    {
        private readonly IPropertyValueExpressionWriter propertyValueExpressionWriter;
        private readonly IExpressionWriter expressionWriter;
        private readonly Readers.Deductive.IPropertyAssignmentReader reader;
        private readonly IPrimitiveSet primitives;

        public PropertyAssignmentWriter(
            IPropertyValueExpressionWriter propertyValueExpressionWriter, 
            IExpressionWriter expressionWriter,
            Readers.Deductive.IPropertyAssignmentReader reader,
            IPrimitiveSet primitives
            )
        {
            this.propertyValueExpressionWriter = propertyValueExpressionWriter;
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.primitives = primitives;
        }

        public IPropertyAssignment Build(Ensemble ensemble, IPropertyAssignmentParameterSet parameters) =>
            new PropertyAssignment().AggregateBuild(
                PropertyAssignmentWriter.CreateGreatGrannies(
                    this.propertyValueExpressionWriter,
                    this.expressionWriter,
                    parameters,
                    primitives
                ),
                new IGreatGrannyProcess<IPropertyAssignment>[]
                {
                    new GreatGrannyProcess<IPropertyValueExpression, IPropertyValueExpressionWriter, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                         ProcessHelper.ParseBuild
                    ),
                    new GreatGrannyProcess<IExpression, IExpressionWriter, IExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.ParseBuild
                    )
                },
                ensemble
            );

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssignment>> CreateGreatGrannies(
            IPropertyValueExpressionWriter propertyValueExpressionWriter,
            IExpressionWriter expressionWriter, 
            IPropertyAssignmentParameterSet parameters,
            IPrimitiveSet primitives
        ) =>
            new IGreatGrannyInfo<IPropertyAssignment>[]
            {
                new IndependentGreatGrannyInfo<IPropertyValueExpression, IPropertyValueExpressionWriter, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                    propertyValueExpressionWriter,
                    () => PropertyAssignmentWriter.CreatePropertyValueExpressionParameterSet(parameters),
                    (g, r) => r.PropertyValueExpression = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionWriter, IExpressionParameterSet, IPropertyAssignment>(
                    expressionWriter,
                    (g) => CreateExpressionParameterSet(primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
            };

        private static IPropertyValueExpressionParameterSet CreatePropertyValueExpressionParameterSet(IPropertyAssignmentParameterSet parameters) =>
          new PropertyValueExpressionParameterSet(
              parameters.Value,
              parameters.Class,
              parameters.ValueMatchBy
          );

        private static ExpressionParameterSet CreateExpressionParameterSet(IPrimitiveSet primitives, IPropertyAssignmentParameterSet parameters, Neuron neuron) =>
            new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        parameters.Property,
                        primitives.Unit
                    ),
                    new UnitParameterSet(
                        neuron,
                        primitives.NominalModifier
                    )
                }
            );

        public IGrannyReader<IPropertyAssignment, IPropertyAssignmentParameterSet> Reader => this.reader;
    }
}
