﻿using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyAssignmentReader : IPropertyAssignmentReader
    {
        private readonly IPropertyValueExpressionReader propertyValueExpressionReader;
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;

        public PropertyAssignmentReader(
            IPropertyValueExpressionReader propertyValueExpressionReader, 
            IExpressionReader expressionReader, 
            IPrimitiveSet primitives
        )
        {
            this.propertyValueExpressionReader = propertyValueExpressionReader;
            this.expressionReader = expressionReader;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyAssignment>> CreateGreatGrannies(
            IPropertyValueExpressionReader propertyValueExpressionReader,
            IExpressionReader expressionReader,
            IPropertyAssignmentParameterSet parameters,
            IPrimitiveSet primitives
        ) =>
            new IGreatGrannyInfo<IPropertyAssignment>[]
            {
                new IndependentGreatGrannyInfo<IPropertyValueExpression, IPropertyValueExpressionReader, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                    propertyValueExpressionReader,
                    () => PropertyAssignmentReader.CreatePropertyValueExpressionParameterSet(parameters),
                    (g, r) => r.PropertyValueExpression = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssignment>(
                    expressionReader,
                    (g) => PropertyAssignmentReader.CreateExpressionParameterSet(primitives, parameters, g.Neuron),
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

        public IEnumerable<IGrannyQuery> GetQueries(IPropertyAssignmentParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IPropertyValueExpression, IPropertyValueExpressionReader, IPropertyValueExpressionParameterSet>(
                    this.propertyValueExpressionReader,
                    (n) => CreatePropertyValueExpressionParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionReader, IExpressionParameterSet>(
                    this.expressionReader,
                    (n) => CreateExpressionParameterSet(this.primitives, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Ensemble ensemble, IPropertyAssignmentParameterSet parameters, out IPropertyAssignment result) =>
            new PropertyAssignment().AggregateTryParse(
                PropertyAssignmentReader.CreateGreatGrannies(
                    this.propertyValueExpressionReader,
                    this.expressionReader,
                    parameters,
                    this.primitives
                ),
                new IGreatGrannyProcess<IPropertyAssignment>[]
                {
                    new GreatGrannyProcess<IPropertyValueExpression, IPropertyValueExpressionReader, IPropertyValueExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyAssignment>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                out result
            );
    }
}