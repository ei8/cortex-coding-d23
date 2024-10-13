using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ValueExpressionReader : IValueExpressionReader
    {
        private readonly IValueReader valueReader;
        private readonly IExpressionReader expressionReader;
        private readonly IPrimitiveSet primitives;

        public ValueExpressionReader(IValueReader valueReader, IExpressionReader expressionReader, IPrimitiveSet primitives)
        {
            this.valueReader = valueReader;
            this.expressionReader = expressionReader;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IValueExpression>> CreateGreatGrannies(
            IValueReader valueReader,
            IExpressionReader expressionReader,
            IValueExpressionParameterSet parameters,
            Ensemble ensemble,
            IPrimitiveSet primitives
        ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IValueExpression>(
                    expressionReader,
                    () => ValueExpressionReader.CreateExpressionParameterSet(primitives, parameters, gc),
                    (g, r) => r.Expression = g
                )
            ).Concat(
                new IGreatGrannyInfo<IValueExpression>[] {
                    new DependentGreatGrannyInfo<IValue, IValueReader, IValueParameterSet, IValueExpression>(
                        valueReader,
                        g => CreateValueParameterSet(
                            parameters,
                            ((IExpression) g).Units.GetValueUnitGranniesByTypeId(primitives.Unit.Id).Single().Value
                            ),
                        (g, r) => r.Value = g
                    )
                }
            );

        private static ExpressionParameterSet CreateExpressionParameterSet(
            IPrimitiveSet primitives,
            IValueExpressionParameterSet parameters,
            Neuron unitGranny
            ) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[] {
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        primitives.Unit
                    ),
                }
            );

        private static IValueParameterSet CreateValueParameterSet(IValueExpressionParameterSet parameters, Neuron value) =>
            new ValueParameterSet(
                value,
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, IValueExpressionParameterSet parameters, out IValueExpression result) =>
            new ValueExpression().AggregateTryParse(
                parameters.Granny,
                ValueExpressionReader.CreateGreatGrannies(
                    this.valueReader,
                    this.expressionReader,
                    parameters,
                    ensemble,
                    this.primitives
                    ),
                new IGreatGrannyProcess<IValueExpression>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IValue, IValueReader, IValueParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                2,
                out result
            );
    }
}
