using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ExpressionWriter : IExpressionWriter
    {
        private readonly IUnitWriter unitWriter;
        private readonly Readers.Deductive.IExpressionReader reader;
        private readonly IPrimitiveSet primitives;

        public ExpressionWriter(
            IUnitWriter unitWriter, 
            Readers.Deductive.IExpressionReader reader, 
            IPrimitiveSet primitives
        )
        {
            this.unitWriter = unitWriter;
            this.reader = reader;
            this.primitives = primitives;
        }

        public IExpression Build(Ensemble ensemble, IExpressionParameterSet parameters) =>
            new Expression().AggregateBuild(
                ExpressionWriter.CreateGreatGrannies(
                    this.unitWriter,
                    parameters
                ),
                parameters.UnitsParameters.Select(
                    u => new GreatGrannyProcess<IUnit, IUnitWriter, IUnitParameterSet, IExpression>(
                        ProcessHelper.ParseBuild
                    )
                ),
                ensemble,
                () => ensemble.AddOrGetIfExists(Neuron.CreateTransient(null, null, null)),
                (r) =>
                    // concat applicable expression types
                    GetExpressionTypes(
                        (id, isEqual) => parameters.UnitsParameters.GetValueUnitParametersByTypeId(id, isEqual).Count(),
                        this.primitives
                    )
                    .Select(et => ensemble.AddOrGetIfExists(et))
                    .Concat(
                        // with Units in result
                        r.Units.Select(u => u.Neuron)
                    )
            );

        private static IEnumerable<IGreatGrannyInfo<IExpression>> CreateGreatGrannies(
            IUnitWriter unitWriter,
            IExpressionParameterSet parameters
        ) =>
            parameters.UnitsParameters.Select(
                u => new IndependentGreatGrannyInfo<IUnit, IUnitWriter, IUnitParameterSet, IExpression>(
                    unitWriter,
                    () => u,
                    (g, r) => r.Units.Add(g)
                )
            );

        internal static IEnumerable<Neuron> GetExpressionTypes(
            Func<Guid, bool, int> headCountRetriever,
            IPrimitiveSet primitives
            )
        {
            var result = new List<Neuron>();

            var headCount = headCountRetriever(primitives.Unit.Id, true);
            var dependentCount = headCountRetriever(primitives.Unit.Id, false);

            if (headCount > 0)
            {
                if (headCount > 1)
                {
                    result.Add(primitives.Coordination);
                }
                else if (headCount == 1 && dependentCount == 0)
                {
                    result.Add(primitives.Simple);
                }
                if (dependentCount > 0)
                {
                    result.Add(primitives.Subordination);
                }
            }
            else
                throw new InvalidOperationException("Expression must have at least one 'Head' unit.");

            return result.ToArray();
        }

        public Readers.Deductive.IGrannyReader<IExpression, IExpressionParameterSet> Reader => this.reader;
    }
}
