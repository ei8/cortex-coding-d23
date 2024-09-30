using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class ValueExpressionReader : IValueExpressionReader
    {
        private readonly IValueReader valueReader;
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;

        public ValueExpressionReader(
            IValueReader valueReader, 
            IExpressionReader expressionReader, 
            IPrimitiveSet primitives
        )
        {
            this.valueReader = valueReader;
            this.expressionReader = expressionReader;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IValueExpression>> CreateGreatGrannies(
            IValueReader valueReader,
            IExpressionReader expressionReader,
            IValueExpressionParameterSet parameters,
            IPrimitiveSet primitives
        ) =>
            new IGreatGrannyInfo<IValueExpression>[]
            {
                new IndependentGreatGrannyInfo<IValue, IValueReader, IValueParameterSet, IValueExpression>(
                    valueReader,
                    () => ValueExpressionReader.CreateValueParameterSet(parameters),
                    (g, r) => r.Value = g
                    ),
                new DependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IValueExpression>(
                    expressionReader,
                    (g) => ValueExpressionReader.CreateExpressionParameterSet(primitives, parameters, g.Neuron),
                    (g, r) => r.Expression = g
                    )
            };

        private static IValueParameterSet CreateValueParameterSet(IValueExpressionParameterSet parameters) =>
            new ValueParameterSet(
                parameters.Value,
                parameters.Class,
                parameters.ValueMatchBy
            );

        public IEnumerable<IGrannyQuery> GetQueries(IValueExpressionParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IValue, IValueReader, IValueParameterSet>(
                    this.valueReader,
                    (n) => CreateValueParameterSet(parameters)
                ),
                new GreatGrannyQuery<IExpression, IExpressionReader, IExpressionParameterSet>(
                    this.expressionReader,
                    (n) => CreateExpressionParameterSet(this.primitives, parameters, n.Last().Neuron)
                )
            };

        private static ExpressionParameterSet CreateExpressionParameterSet(IPrimitiveSet primitives, IValueExpressionParameterSet parameters, Neuron n) =>
            new ExpressionParameterSet(
                new[] {
                    new UnitParameterSet(
                        n,
                        primitives.Unit
                    ),
                }
            );

        public bool TryParse(Ensemble ensemble, IValueExpressionParameterSet parameters, out IValueExpression result) =>
            new ValueExpression().AggregateTryParse(
                ValueExpressionReader.CreateGreatGrannies(
                    this.valueReader,
                    this.expressionReader,
                    parameters,
                    this.primitives
                ),
                new IGreatGrannyProcess<IValueExpression>[]
                {
                    new GreatGrannyProcess<IValue, IValueReader, IValueParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                out result
            );
    }
}
