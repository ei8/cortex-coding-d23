﻿using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class PropertyValueExpressionReader : IPropertyValueExpressionReader
    {
        private readonly IValueExpressionReader valueExpressionReader;
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;

        public PropertyValueExpressionReader(
            IValueExpressionReader valueExpressionReader, 
            IExpressionReader expressionReader, 
            IPrimitiveSet primitives
        )
        {
            this.valueExpressionReader = valueExpressionReader;
            this.expressionReader = expressionReader;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IPropertyValueExpression>> CreateGreatGrannies(
            IValueExpressionReader valueExpressionReader,
            IExpressionReader expressionReader,
            IPropertyValueExpressionParameterSet parameters,
            IPrimitiveSet primitives
        ) =>
            new IGreatGrannyInfo<IPropertyValueExpression>[]
            {
                new IndependentGreatGrannyInfo<IValueExpression, IValueExpressionReader, IValueExpressionParameterSet, IPropertyValueExpression>(
                    valueExpressionReader,
                    () => PropertyValueExpressionReader.CreateValueExpressionParameterSet(parameters),
                    (g, r) => r.ValueExpression = g
                ),
                new DependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyValueExpression>(
                    expressionReader,
                    (g) => PropertyValueExpressionReader.CreateExpressionParameterSet(primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                )
            };

        private static ExpressionParameterSet CreateExpressionParameterSet(IPrimitiveSet primitives, IPropertyValueExpressionParameterSet parameters, Neuron neuron) =>
            new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        neuron,
                        primitives.Unit
                    ),
                    new UnitParameterSet(
                        primitives.Of,
                        primitives.Case
                    )
                }
            );

        private static IValueExpressionParameterSet CreateValueExpressionParameterSet(IPropertyValueExpressionParameterSet parameters) =>
            new ValueExpressionParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        public IEnumerable<IGrannyQuery> GetQueries(IPropertyValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IValueExpression, IValueExpressionReader, IValueExpressionParameterSet>(
                    this.valueExpressionReader,
                    (n) => PropertyValueExpressionReader.CreateValueExpressionParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionReader, IExpressionParameterSet>(
                    this.expressionReader,
                    (n) => PropertyValueExpressionReader.CreateExpressionParameterSet(this.primitives, parameters, n.Last().Neuron)
                )
            };

        public bool TryParse(Ensemble ensemble, IPropertyValueExpressionParameterSet parameters, out IPropertyValueExpression result) =>
            new PropertyValueExpression().AggregateTryParse(
                PropertyValueExpressionReader.CreateGreatGrannies(
                    this.valueExpressionReader,
                    this.expressionReader,
                    parameters,
                    this.primitives
                ),
                new IGreatGrannyProcess<IPropertyValueExpression>[]
                {
                    new GreatGrannyProcess<IValueExpression, IValueExpressionReader, IValueExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IPropertyValueExpression>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                out result
            );
    }
}