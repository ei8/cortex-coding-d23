using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ValueExpressionProcessor : IValueExpressionProcessor
    {
        private readonly IValueProcessor valueProcessor;
        private readonly IExpressionProcessor expressionProcessor;
        private readonly IPrimitiveSet primitives;

        public ValueExpressionProcessor(IValueProcessor valueProcessor, IExpressionProcessor expressionProcessor, IPrimitiveSet primitives)
        {
            this.valueProcessor = valueProcessor;
            this.expressionProcessor = expressionProcessor;
            this.primitives = primitives;
        }

        private static IEnumerable<IGreatGrannyInfo<IValueExpression>> CreateGreatGrannies(
            IValueProcessor valueProcessor,
            IExpressionProcessor expressionProcessor,
            IValueExpressionParameterSet parameters,
            Ensemble ensemble,
            IPrimitiveSet primitives
            ) =>
                ProcessHelper.CreateGreatGrannyCandidates(
                    ensemble,
                    parameters.Granny,
                    gc => new IndependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        expressionProcessor,
                        () => ValueExpressionProcessor.CreateExpressionParameterSet(primitives, parameters, gc),
                        (g, r) => r.Expression = g
                    )
                ).Concat(
                    new IGreatGrannyInfo<IValueExpression>[] {
                        new DependentGreatGrannyInfo<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                            valueProcessor,
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
                ValueExpressionProcessor.CreateGreatGrannies(
                    valueProcessor,
                    expressionProcessor,
                    parameters,
                    ensemble,
                    this.primitives
                    ),
                new IGreatGrannyProcess<IValueExpression>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        )
                },
                ensemble,
                2,
                out result
            );
    }
}
