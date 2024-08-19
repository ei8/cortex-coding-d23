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
                    ensemble.GetPostsynapticNeurons(parameters.Granny.Id).SingleOrDefault(n => n.Id != options.Primitives.Simple.Id),
                    gc => new GreatGrannyInfo<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
                        expressionProcessor,
                        (g) => ValueExpressionProcessor.CreateExpressionParameterSet(options.Primitives, parameters, gc),
                        (g, r) => r.Expression = g
                        )
                    );
                //    new GreatGrannyReadInfo<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                //        valueProcessor,
                //        CreateValueParameterSet(parameters),
                //        (g, r) => r.Value = g
                //        )
                //};

        private static ExpressionParameterSet CreateExpressionParameterSet(
            PrimitiveSet primitives, 
            IValueExpressionParameterSet parameters, 
            Neuron unitGranny) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[] {
                    UnitParameterSet.CreateWithGrannyAndType(
                        unitGranny,
                        primitives.Unit
                    ),
                }
            );

        private static IValueParameterSet CreateValueParameterSet(IValueExpressionParameterSet parameters) =>
            new ValueParameterSet(
                parameters.Granny,
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
                    new GreatGrannyProcess<IValue, IValueProcessor, IValueParameterSet, IValueExpression>(
                        ProcessHelper.TryParse
                        ),
                    new GreatGrannyProcess<IExpression, IExpressionProcessor, IExpressionParameterSet, IValueExpression>(
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
