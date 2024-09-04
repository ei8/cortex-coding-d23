using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class ValueExpressionProcessor : IValueExpressionProcessor
    {
        private readonly IValueProcessor valueProcessor;
        private readonly IExpressionProcessor expressionProcessor;

        public ValueExpressionProcessor(IValueProcessor valueProcessor, IExpressionProcessor expressionProcessor)
        {
            this.valueProcessor = valueProcessor;
            this.expressionProcessor = expressionProcessor;
        }

        private static IEnumerable<IGreatGrannyInfo<IValueExpression>> CreateGreatGrannies(
            IValueProcessor valueProcessor,
            IExpressionProcessor expressionProcessor,
            Id23neurULizerReadOptions options,
            IValueExpressionParameterSet parameters,
            Ensemble ensemble
            ) =>
                ProcessHelper.CreateGreatGrannyCandidates(
                    ensemble,
                    parameters.Granny,
                    gc => new IndependentGreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        expressionProcessor,
                        () => CreateExpressionParameterSet(options.Primitives, parameters, gc),
                        (g, r) => r.Expression = g
                    )
                ).Concat(
                    new IGreatGrannyInfo<IValueExpression>[] {
                        new DependentGreatGrannyInfo<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                            valueProcessor,
                            g => CreateValueParameterSet(
                                parameters,
                                ((IExpression) g).Units.GetValueUnitGranniesByTypeId(options.Primitives.Unit.Id).Single().Value
                                ),
                            (g, r) => r.Value = g
                        )
                    }
                );

        private static ExpressionParameterSet CreateExpressionParameterSet(
            PrimitiveSet primitives,
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

        public bool TryParse(Ensemble ensemble, Id23neurULizerReadOptions options, IValueExpressionParameterSet parameters, out IValueExpression result) =>
            new ValueExpression().AggregateTryParse(
                parameters.Granny,
                CreateGreatGrannies(
                    valueProcessor,
                    expressionProcessor,
                    options,
                    parameters,
                    ensemble
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
                options,
                2,
                out result
            );
    }
}
