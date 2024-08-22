using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
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
                    gc => new GreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        expressionProcessor,
                        (g) => ValueExpressionProcessor.CreateExpressionParameterSet(options.Primitives, parameters, gc),
                        (g, r) => r.Expression = g
                    )
                ).Concat(
                    new IGreatGrannyInfo<IValueExpression>[] {
                        new GreatGrannyInfo<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                            valueProcessor,
                            g => ValueExpressionProcessor.CreateValueParameterSet(
                                parameters, 
                                ((IExpression) g).Units.GetByTypeId(options.Primitives.Unit.Id).Single().Value
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
                ValueExpressionProcessor.CreateGreatGrannies(
                    this.valueProcessor, 
                    this.expressionProcessor,
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
