using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class ExpressionWriter : IExpressionWriter
    {
        private const string ExpressionTypePostsynapticInfoName = "ExpressionType";
        private readonly IUnitWriter unitWriter;
        private readonly Readers.Deductive.IExpressionReader reader;
        private readonly IExternalReferenceSet externalReferences;

        public ExpressionWriter(
            IUnitWriter unitWriter, 
            Readers.Deductive.IExpressionReader reader, 
            IExternalReferenceSet externalReferences
        )
        {
            this.unitWriter = unitWriter;
            this.reader = reader;
            this.externalReferences = externalReferences;
        }

        public IExpression Build(Network network, IExpressionParameterSet parameters) =>
            new Expression().AggregateBuild(
                ExpressionWriter.CreateGreatGrannies(
                    this.unitWriter,
                    parameters
                ),
                parameters.UnitParameters.Select(
                    u => new GreatGrannyProcess<IUnit, IUnitWriter, IUnitParameterSet, IExpression>(
                        ProcessHelper.ParseBuild
                    )
                ),
                network,
                () => network.AddOrGetIfExists(Neuron.CreateTransient(null, null, null)),
                (r) =>
                    // concat applicable expression types
                    GetExpressionTypes(
                        (id, isEqual) => parameters.UnitParameters.GetValueUnitParametersByTypeId(id, isEqual).Count(),
                        this.externalReferences
                    )
                    .Select(et => new PostsynapticInfo() {
                        Name = ExpressionWriter.ExpressionTypePostsynapticInfoName,
                        Neuron = network.AddOrGetIfExists(et)
                    })
                    .Concat(
                        // with Units in result
                        r.ToPostsynapticInfos(r.Units, g => r.Units)
                    )
            );

        private static IEnumerable<IGreatGrannyInfo<IExpression>> CreateGreatGrannies(
            IUnitWriter unitWriter,
            IExpressionParameterSet parameters
        ) =>
            parameters.UnitParameters.Select(
                u => new IndependentGreatGrannyInfo<IUnit, IUnitWriter, IUnitParameterSet, IExpression>(
                    unitWriter,
                    () => u,
                    (g, r) => r.Units.Add(g)
                )
            );

        internal static IEnumerable<Neuron> GetExpressionTypes(
            Func<Guid, bool, int> headCountRetriever,
            IExternalReferenceSet externalReferences
            )
        {
            var result = new List<Neuron>();

            var headCount = headCountRetriever(externalReferences.Unit.Id, true);
            var dependentCount = headCountRetriever(externalReferences.Unit.Id, false);

            if (headCount > 0)
            {
                if (headCount > 1)
                {
                    result.Add(externalReferences.Coordination);
                }
                else if (headCount == 1 && dependentCount == 0)
                {
                    result.Add(externalReferences.Simple);
                }
                if (dependentCount > 0)
                {
                    result.Add(externalReferences.Subordination);
                }
            }
            else
                throw new InvalidOperationException("Expression must have at least one 'Head' unit.");

            return result.ToArray();
        }

        public Readers.Deductive.IGrannyReader<IExpression, IExpressionParameterSet> Reader => this.reader;
    }
}
